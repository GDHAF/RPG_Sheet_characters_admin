using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;
using System.Text.Json;
using RPG_Sheet_characters_admin.Dto;
using RPG_Sheet_characters_admin.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage;

namespace RPG_Sheet_characters_admin.Controllers
{
    [Authorize]
    public class InserirController : Controller
    {

        private readonly ScareContext _context;
        private readonly IWebHostEnvironment _environment;

        public InserirController(ScareContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        //Atualização de personagem com proteção contra SQL Injection
        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> AtualizarPersonagem(int id, [FromBody] AtualizarPersonagemDto dto)
        {
            if (dto == null || id != dto.Id)
            {
                return BadRequest("Os dados fornecidos são inválidos ou os IDs não coincidem.");
            }

            // Configurando estritamente todos os parâmetros conforme a assinatura da sua procedure
            var parametros = new[]
            {
                new MySqlParameter("@p_id_personagem", dto.Id),
                new MySqlParameter("@p_nome", dto.Nome),
                new MySqlParameter("@p_nivel_fis", dto.NivelFis),
                new MySqlParameter("@p_nivel_psi", dto.NivelPsi),
                new MySqlParameter("@p_origem", dto.Origem),
                new MySqlParameter("@p_idade", dto.Idade),
                new MySqlParameter("@p_classe", dto.Classe),
                new MySqlParameter("@p_patente", dto.Patente),

                // personagem_info
                new MySqlParameter("@p_descricao", dto.Descricao ?? (object)DBNull.Value),
                new MySqlParameter("@p_historia", dto.Historia ?? (object)DBNull.Value),
                new MySqlParameter("@p_notes", dto.Notes ?? (object)DBNull.Value),
                new MySqlParameter("@p_armadura", dto.Armadura ?? (object)DBNull.Value),
                new MySqlParameter("@p_arma", dto.Arma ?? (object)DBNull.Value),
                new MySqlParameter("@p_def_arma", dto.DefArma),
                new MySqlParameter("@p_alcance", dto.Alcance),
                new MySqlParameter("@p_esquivar", dto.Esquivar ?? (object)DBNull.Value),
                new MySqlParameter("@p_atacar", dto.Atacar ?? (object)DBNull.Value),
                new MySqlParameter("@p_interromper", dto.Interromper ?? (object)DBNull.Value),
                new MySqlParameter("@p_dano", dto.Dano ?? (object)DBNull.Value),
                new MySqlParameter("@p_oficios", dto.Oficios ?? (object)DBNull.Value),
                new MySqlParameter("@p_proficiencias", dto.Proficiencias ?? (object)DBNull.Value),
                new MySqlParameter("@p_image_name", dto.ImageName ?? (object)DBNull.Value),
                new MySqlParameter("@p_image_data", dto.ImageData ?? (object)DBNull.Value),

                // Serialização das listas em Strings JSON para o MySQL interpretar via JSON_TABLE
                new MySqlParameter("@p_pericias", dto.Pericias != null ? JsonSerializer.Serialize(dto.Pericias) : (object)DBNull.Value),
                new MySqlParameter("@p_recursos_vitais", dto.RecursosVitais != null ? JsonSerializer.Serialize(dto.RecursosVitais) : (object)DBNull.Value),
                new MySqlParameter("@p_atributos", dto.Atributos != null ? JsonSerializer.Serialize(dto.Atributos) : (object)DBNull.Value),
                new MySqlParameter("@p_ataques", dto.Ataques != null ? JsonSerializer.Serialize(dto.Ataques) : (object)DBNull.Value),
                new MySqlParameter("@p_emocoes", dto.Emocoes != null ? JsonSerializer.Serialize(dto.Emocoes) : (object)DBNull.Value),
                new MySqlParameter("@p_sanidade", dto.Sanidade != null ? JsonSerializer.Serialize(dto.Sanidade) : (object)DBNull.Value)
            };

            try
            {
                // Executa a procedure passando a lista de parâmetros mapeada acima
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_atualizar_personagem(@p_id_personagem, @p_nome, @p_nivel_fis, @p_nivel_psi, @p_origem, @p_idade, @p_classe, @p_patente, " +
                    "@p_descricao, @p_historia, @p_notes, @p_armadura, @p_arma, @p_def_arma, @p_alcance, @p_esquivar, @p_atacar, @p_interromper, @p_dano, " +
                    "@p_oficios, @p_proficiencias, @p_image_name, @p_image_data, @p_pericias, @p_recursos_vitais, @p_atributos, @p_ataques, @p_emocoes, @p_sanidade)",
                    parametros);

                return NoContent(); // Sucesso sem conteúdo (padrão HTTP 204 para Updates bem-sucedidos)
            }
            catch (MySqlException ex) when (ex.SqlState == "45000")
            {
                // Captura o SIGNAL de erro customizado definido na sua procedure ('Personagem não encontrado')
                return NotFound(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Falha crítica ao executar a atualização da ficha.", erro = ex.Message });
            }

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("CriarFicha", new CriarFichaModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CriarFichaModel model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState
                    .Where(kv => kv.Value.Errors.Count > 0)
                    .Select(kv => $"{kv.Key}: {string.Join("; ", kv.Value.Errors.Select(e => e.ErrorMessage))}");
                ModelState.AddModelError(string.Empty, string.Join(" | ", erros)); // temporary, for debugging
                return View("CriarFicha", model);
            }

            //Definição da Imagem
            string imageName = "Gamma_Future_Simbolo.png";
            byte[]? image = null;

            if (model.Imagem != null && model.Imagem.Length > 0)
            {
                imageName = Path.GetFileName(model.Imagem.FileName);

                using (var memoryStream = new MemoryStream())
                {
                    await model.Imagem.CopyToAsync(memoryStream);
                    image = memoryStream.ToArray();
                }
            }
            else
            {
                var defaultImagePath = Path.Combine(
                    _environment.WebRootPath,
                    "img",
                    "Gamma_Future_Simbolo.png"
                );

                await using var fileStream = new FileStream(
                    defaultImagePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read
                );

                using var memoryStream = new MemoryStream();

                await fileStream.CopyToAsync(memoryStream);

                image = memoryStream.ToArray();
            }

            //Parâmetro de Saída da Procedure
            var outIdParam = new MySqlParameter("@p_id_personagem", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };

            //Configuração dos parâmetros de entrada da Procedure
            var parametros = new[]
            {
                //Personagem
                new MySqlParameter("@p_jogador", User.Identity?.Name ?? String.Empty),
                new MySqlParameter("@p_nome", model.Nome),
                new MySqlParameter("@p_origem", model.Origem),
                new MySqlParameter("@p_idade", model.Idade),
                new MySqlParameter("@p_classe", model.Classe),

                //Personagem_Info
                new MySqlParameter("@p_descricao", model.Descricao ?? (object)DBNull.Value),
                new MySqlParameter("@p_historia", model.Historia ?? (object)DBNull.Value),
                new MySqlParameter("@p_armadura", model.Armadura ?? (object)DBNull.Value),
                new MySqlParameter("@p_arma", model.Arma ?? (object)DBNull.Value),
                new MySqlParameter("@p_def_arma", model.Def_arma),
                new MySqlParameter("@p_alcance", model.Alcance),
                new MySqlParameter("@p_esquivar", model.Esquivar ?? (object)DBNull.Value),
                new MySqlParameter("@p_atacar", model.Atacar ?? (object)DBNull.Value),
                new MySqlParameter("@p_interromper", model.Interromper ?? (object)DBNull.Value),
                new MySqlParameter("@p_dano", model.Dano ?? (object)DBNull.Value),
                new MySqlParameter("@p_oficios", model.Oficio ?? (object)DBNull.Value),
                new MySqlParameter("@p_proficiencias", model.Proficiencia ?? (object)DBNull.Value),

                //Atributos
                new MySqlParameter("@p_atributos", model.Atributos.Count > 0 ? JsonSerializer.Serialize(model.Atributos) : (object)DBNull.Value),
                new MySqlParameter("@p_pericias", model.Pericias.Count > 0 ? JsonSerializer.Serialize(model.Pericias) : (object)DBNull.Value),
                new MySqlParameter("@p_recursos_vitais", model.Recursos.Count > 0 ? JsonSerializer.Serialize(model.Recursos) : (object)DBNull.Value),
                new MySqlParameter("@p_ataques", model.Ataques.Count > 0 ? JsonSerializer.Serialize(model.Ataques) : (object)DBNull.Value),
                new MySqlParameter("@p_emocoes", model.Emocoes.Count > 0 ? JsonSerializer.Serialize(model.Emocoes) : (object)DBNull.Value),
                new MySqlParameter("@p_sanidade", model.Sanidades.Count > 0 ? JsonSerializer.Serialize(model.Sanidades) : (object)DBNull.Value),

                //Imagem
                new MySqlParameter("@p_image_name", imageName),
                new MySqlParameter("@p_image_data", image ?? (object)DBNull.Value),
                outIdParam
            };

            try
            {
                object id_pers;
                using (var comando = _context.Database.GetDbConnection().CreateCommand())
                {
                    comando.CommandText = "sp_criar_personagem";
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

                    foreach (var parametro in parametros)
                    {
                        comando.Parameters.Add(parametro);
                    }

                    if(comando.Connection.State != System.Data.ConnectionState.Open)
                    {
                        await comando.Connection.OpenAsync();
                    }

                    await comando.ExecuteNonQueryAsync();

                    id_pers = parametros.FirstOrDefault(p => p.ParameterName == "@p_id_personagem")?.Value;
                }

                return RedirectToAction("Agente", "Agentes", new { id = id_pers });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao criar personagem: {ex.Message}");
                return View("CriarFicha", model);
            }


        }

    }
}
