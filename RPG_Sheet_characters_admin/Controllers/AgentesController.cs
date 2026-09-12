using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySqlConnector;
using System.Text.Json;
using RPG_Sheet_characters_admin.Models;
using System.ComponentModel;

namespace RPG_Sheet_characters_admin.Controllers
{
    [Authorize]
    public class AgentesController : Controller
    {
        private readonly ScareContext _context;

        public AgentesController(ScareContext context)
        {
            _context = context;
        }

        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        //          Banco de Agentes
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        public IActionResult Banco_Agentes()
        {
            return View("Banco-de-Agentes", new List<CriarFichaModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Listagem()
        {

            var agentes = new List<CriarFichaModel>();

            try
            {
                using (var comando = _context.Database.GetDbConnection().CreateCommand())
                {

                    comando.CommandText = "sp_listar_agentes";
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
                    comando.Parameters.Add(new MySqlParameter("@p_jogador", User.Identity?.Name ?? String.Empty));

                    if (comando.Connection.State != System.Data.ConnectionState.Open)
                    {
                        await comando.Connection.OpenAsync();
                    }

                    using (var reader = await comando.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var agente = new CriarFichaModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Nome = reader.IsDBNull(reader.GetOrdinal("nome")) ? null : reader.GetString(reader.GetOrdinal("nome")),
                                Cod_Agente = reader.IsDBNull(reader.GetOrdinal("cod_agente")) ? null : reader.GetString(reader.GetOrdinal("cod_agente")),
                                Patente = reader.IsDBNull(reader.GetOrdinal("patente")) ? null : reader.GetString(reader.GetOrdinal("patente")),
                                Status = reader.IsDBNull(reader.GetOrdinal("estatus")) ? null : reader.GetString(reader.GetOrdinal("estatus")),
                                Image_Name = reader.IsDBNull(reader.GetOrdinal("image_name")) ? null : reader.GetString(reader.GetOrdinal("image_name")),
                                Image_Data = reader.IsDBNull(reader.GetOrdinal("image_data")) ? null : (byte[])reader["image_data"]
                            };
                            agentes.Add(agente);
                        }
                    }
                    

                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao listar Agentes: {ex.Message}");
                return View("Banco-de-Agentes", agentes);
            }

            return View("Banco-de-Agentes", agentes);
        }


        // =-=-=-==-=-=-=-=-=-=-=-=-=-=-=-=-=-
        //               Ficha
        // =-=-=-==-=-=-=-=-=-=-=-=-=-=-=-=-=-

        [HttpGet]
        public async Task<IActionResult> Agente(int id)
        {
            var model = new CriarFichaModel();

            try
            {
                using (var comando = _context.Database.GetDbConnection().CreateCommand())
                {
                    comando.CommandText = "sp_buscar_personagem";
                    comando.CommandType = System.Data.CommandType.StoredProcedure;

                    comando.Transaction =
                        _context.Database.CurrentTransaction?.GetDbTransaction();

                    comando.Parameters.Add(new MySqlParameter("@p_id_personagem", id));

                    if (comando.Connection.State != System.Data.ConnectionState.Open)
                    {
                        await comando.Connection.OpenAsync();
                    }

                    using (var reader = await comando.ExecuteReaderAsync())
                    {
                        // ============================================================
                        // 1. DADOS PRINCIPAIS + PERSONAGEM_INFO
                        // ============================================================

                        if (!await reader.ReadAsync())
                        {
                            return NotFound();
                        }

                        model.Id = reader.IsDBNull(reader.GetOrdinal("id")) ? null : reader.GetInt32(reader.GetOrdinal("id"));

                        model.Jogador = reader.IsDBNull(reader.GetOrdinal("jogador")) ? null : reader.GetString(reader.GetOrdinal("jogador"));

                        model.Cod_Agente = reader.IsDBNull(reader.GetOrdinal("cod_agente")) ? null : reader.GetString(reader.GetOrdinal("cod_agente"));

                        model.Nome = reader.IsDBNull(reader.GetOrdinal("nome")) ? string.Empty : reader.GetString(reader.GetOrdinal("nome"));

                        model.Nivel_Fis = reader.IsDBNull(reader.GetOrdinal("nivel_fisico")) ? null : reader.GetInt32(reader.GetOrdinal("nivel_fisico"));

                        model.Nivel_Psi = reader.IsDBNull(reader.GetOrdinal("nivel_psi")) ? null : reader.GetInt32(reader.GetOrdinal("nivel_psi"));

                        model.Origem = reader.IsDBNull(reader.GetOrdinal("origem")) ? null : reader.GetString(reader.GetOrdinal("origem"));

                        model.Idade = reader.IsDBNull(reader.GetOrdinal("idade")) ? null : reader.GetInt32(reader.GetOrdinal("idade"));

                        model.Classe = reader.IsDBNull(reader.GetOrdinal("classe")) ? null : reader.GetString(reader.GetOrdinal("classe"));

                        model.Patente = reader.IsDBNull(reader.GetOrdinal("patente")) ? null : reader.GetString(reader.GetOrdinal("patente"));

                        model.Equipe = reader.IsDBNull(reader.GetOrdinal("equipe")) ? null : reader.GetString(reader.GetOrdinal("equipe"));

                        model.Status = reader.IsDBNull(reader.GetOrdinal("estatus")) ? null : reader.GetString(reader.GetOrdinal("estatus"));

                        model.Descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? null : reader.GetString(reader.GetOrdinal("descricao"));

                        model.Historia = reader.IsDBNull(reader.GetOrdinal("historia")) ? null : reader.GetString(reader.GetOrdinal("historia"));

                        model.Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes"));

                        model.Infracoes = reader.IsDBNull(reader.GetOrdinal("infra")) ? null : reader.GetString(reader.GetOrdinal("infra"));

                        model.Armadura = reader.IsDBNull(reader.GetOrdinal("armadura")) ? null : reader.GetString(reader.GetOrdinal("armadura"));

                        model.Arma = reader.IsDBNull(reader.GetOrdinal("arma")) ? null : reader.GetString(reader.GetOrdinal("arma"));

                        model.Def_arma = reader.IsDBNull(reader.GetOrdinal("def_arma")) ? null : reader.GetInt32(reader.GetOrdinal("def_arma"));

                        model.Alcance = reader.IsDBNull(reader.GetOrdinal("alcance")) ? null : reader.GetInt32(reader.GetOrdinal("alcance"));

                        model.Esquivar = reader.IsDBNull(reader.GetOrdinal("esquivar")) ? null : reader.GetString(reader.GetOrdinal("esquivar"));

                        model.Atacar = reader.IsDBNull(reader.GetOrdinal("atacar")) ? null : reader.GetString(reader.GetOrdinal("atacar"));

                        model.Interromper = reader.IsDBNull(reader.GetOrdinal("interromper")) ? null : reader.GetString(reader.GetOrdinal("interromper"));

                        model.Dano = reader.IsDBNull(reader.GetOrdinal("dano")) ? null : reader.GetString(reader.GetOrdinal("dano"));

                        model.Oficio = reader.IsDBNull(reader.GetOrdinal("oficios")) ? null : reader.GetString(reader.GetOrdinal("oficios"));

                        model.Proficiencia = reader.IsDBNull(reader.GetOrdinal("proficiencias")) ? null : reader.GetString(reader.GetOrdinal("proficiencias"));

                        model.Image_Name = reader.IsDBNull(reader.GetOrdinal("image_name")) ? null : reader.GetString(reader.GetOrdinal("image_name"));

                        model.Image_Data = reader.IsDBNull(reader.GetOrdinal("image_data")) ? null : (byte[])reader["image_data"];


                        // ============================================================
                        // 2. ATRIBUTOS
                        // ============================================================

                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            var atributo = new AtributoFormItem
                            {
                                Atributo = reader.IsDBNull(reader.GetOrdinal("atributo")) ? null : reader.GetString(reader.GetOrdinal("atributo")),

                                Pontos = reader.IsDBNull(reader.GetOrdinal("pontos")) ? null : reader.GetInt32(reader.GetOrdinal("pontos"))
                            };

                            model.Atributos.Add(atributo);
                        }


                        // ============================================================
                        // 3. RECURSOS VITAIS
                        // ============================================================

                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            var recurso = new RecursoFormItem
                            {
                                TipoRecurso = reader.IsDBNull(reader.GetOrdinal("tipo_recurso")) ? null : reader.GetString(reader.GetOrdinal("tipo_recurso")),

                                Maximo = reader.IsDBNull(reader.GetOrdinal("maximo")) ? null : reader.GetInt32(reader.GetOrdinal("maximo")),
                                
                                Atual = reader.IsDBNull(reader.GetOrdinal("atual")) ? null : reader.GetInt32(reader.GetOrdinal("atual"))
                            };

                            model.Recursos.Add(recurso);
                        }


                        // ============================================================
                        // 4. ATAQUES
                        // ============================================================

                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            var ataque = new AtaquesFormItem
                            {
                                Categoria = reader.IsDBNull(reader.GetOrdinal("categoria")) ? null : reader.GetString(reader.GetOrdinal("categoria")),

                                Tipo = reader.IsDBNull(reader.GetOrdinal("tipo")) ? null : reader.GetString(reader.GetOrdinal("tipo")),

                                Bonus = reader.IsDBNull(reader.GetOrdinal("bonus")) ? null : reader.GetInt32(reader.GetOrdinal("bonus")),

                                Extra = reader.IsDBNull(reader.GetOrdinal("extra")) ? null : reader.GetString(reader.GetOrdinal("extra"))
                            };

                            model.Ataques.Add(ataque);
                        }


                        // ============================================================
                        // 5. PERÍCIAS
                        // ============================================================

                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            var pericia = new PericiaFormItem
                            {
                                Pericia = reader.IsDBNull(reader.GetOrdinal("pericia")) ? null : reader.GetString(reader.GetOrdinal("pericia")),

                                Outros = reader.IsDBNull(reader.GetOrdinal("outros")) ? null : reader.GetInt32(reader.GetOrdinal("outros")),

                                Treinado = reader.IsDBNull(reader.GetOrdinal("treinado")) ? null : reader.GetBoolean(reader.GetOrdinal("treinado"))
                            };

                            model.Pericias.Add(pericia);
                        }


                        // ============================================================
                        // 6. EMOÇÕES
                        // ============================================================

                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            var emocao = new EmocoesFormItem
                            {
                                Emocao = reader.IsDBNull(reader.GetOrdinal("emocao")) ? null : reader.GetString(reader.GetOrdinal("emocao")),

                                Ranking = reader.IsDBNull(reader.GetOrdinal("ranking")) ? null : reader.GetString(reader.GetOrdinal("ranking"))
                            };

                            model.Emocoes.Add(emocao);
                        }


                        // ============================================================
                        // 7. INVENTÁRIO
                        // ============================================================

                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            var item = new ItensFormItem
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("id_item")) ? 0 : reader.GetInt32(reader.GetOrdinal("id_item")),
                                Tipo = reader.IsDBNull(reader.GetOrdinal("tipo")) ? null : reader.GetString(reader.GetOrdinal("tipo")),

                                Item = reader.IsDBNull(reader.GetOrdinal("item")) ? null : reader.GetString(reader.GetOrdinal("item")),

                                Qtd = reader.IsDBNull(reader.GetOrdinal("qtd")) ? null : reader.GetInt32(reader.GetOrdinal("qtd"))
                            };

                            model.Itens.Add(item);
                        }


                        // ============================================================
                        // 8. EQUIPAMENTOS
                        // ============================================================

                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            var item = new ItensFormItem
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("id_item")) ? 0 : reader.GetInt32(reader.GetOrdinal("id_item")),
                                Tipo = reader.IsDBNull(reader.GetOrdinal("tipo")) ? null : reader.GetString(reader.GetOrdinal("tipo")),

                                Item = reader.IsDBNull(reader.GetOrdinal("item")) ? null : reader.GetString(reader.GetOrdinal("item")),

                                Qtd = reader.IsDBNull(reader.GetOrdinal("qtd")) ? null : reader.GetInt32(reader.GetOrdinal("qtd"))
                            };

                            model.Itens.Add(item);
                        }


                        // ============================================================
                        // 9. SANIDADE / SITUAÇÕES
                        // ============================================================

                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            var sanidade = new SituacoesFormItem
                            {
                                Situacao = reader.IsDBNull(reader.GetOrdinal("situacao")) ? null : reader.GetString(reader.GetOrdinal("situacao")),

                                Nivel = reader.IsDBNull(reader.GetOrdinal("nivel")) ? null : reader.GetString(reader.GetOrdinal("nivel"))
                            };

                            model.Sanidades.Add(sanidade);
                        }


                        // ============================================================
                        // 10. HABILIDADES
                        // ============================================================

                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            var habilidade = new HabilidadeFormItem
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("id_hability")) ? 0 : reader.GetInt32(reader.GetOrdinal("id_hability")),
                                Habilidade = reader.IsDBNull(reader.GetOrdinal("habilidade")) ? null : reader.GetString(reader.GetOrdinal("habilidade")),

                                Custo = reader.IsDBNull(reader.GetOrdinal("custo")) ? null : reader.GetInt32(reader.GetOrdinal("custo")),

                                Alcance = reader.IsDBNull(reader.GetOrdinal("alcance")) ? null : reader.GetInt32(reader.GetOrdinal("alcance")),

                                Aprimorado = reader.IsDBNull(reader.GetOrdinal("aprimorar")) ? null : reader.GetBoolean(reader.GetOrdinal("aprimorar")),

                                Efeito = reader.IsDBNull(reader.GetOrdinal("efeito")) ? null : reader.GetString(reader.GetOrdinal("efeito")),

                                Arma = reader.IsDBNull(reader.GetOrdinal("arma")) ? null : reader.GetBoolean(reader.GetOrdinal("arma")),

                                Acao = reader.IsDBNull(reader.GetOrdinal("acao")) ? null : reader.GetBoolean(reader.GetOrdinal("acao"))
                            };

                            model.Habilidades.Add(habilidade);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    $"Erro ao buscar Agente: {ex.Message}"
                );

                return View("Dossie", model);
            }

            return View("Dossie", model);
        }

        [HttpPut]
        [Route("Agentes/Atualizar")]
        public async Task<IActionResult> Atualizar([FromBody] CriarFichaModel model)
        {
            if (model == null)
            {
                var formatErrors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest($"Falha na estrutura dos dados JSON recebidos. Detalhes: {formatErrors}");
            }

            if (model.Id == null || model.Id == 0)
            {
                return BadRequest("ID do personagem inválido.");
            }

            try
            {

                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Agente", model.Id);
                }

                byte[]? imageData = model.Image_Data;
                string? imageName = model.Image_Name;

                using (var comando = _context.Database.GetDbConnection().CreateCommand())
                {

                    comando.CommandText = "sp_atualizar_personagem";
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

                    // Parâmetros Básicos
                    comando.Parameters.Add(new MySqlParameter("@p_id_personagem", model.Id));
                    comando.Parameters.Add(new MySqlParameter("@p_nome", model.Nome));
                    comando.Parameters.Add(new MySqlParameter("@p_cod_agente", model.Cod_Agente));
                    comando.Parameters.Add(new MySqlParameter("@p_nivel_fis", model.Nivel_Fis));
                    comando.Parameters.Add(new MySqlParameter("@p_nivel_psi", model.Nivel_Psi));
                    comando.Parameters.Add(new MySqlParameter("@p_origem", model.Origem));
                    comando.Parameters.Add(new MySqlParameter("@p_idade", model.Idade));
                    comando.Parameters.Add(new MySqlParameter("@p_classe", model.Classe));
                    comando.Parameters.Add(new MySqlParameter("@p_patente", model.Patente));
                    comando.Parameters.Add(new MySqlParameter("@p_equipe", model.Equipe));
                    comando.Parameters.Add(new MySqlParameter("@p_estatus", model.Status));

                    // Informações do Personagem
                    comando.Parameters.Add(new MySqlParameter("@p_descricao", model.Descricao ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_historia", model.Historia ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_notes", model.Notes ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_infra", model.Infracoes ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_armadura", model.Armadura ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_arma", model.Arma ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_def_arma", model.Def_arma));
                    comando.Parameters.Add(new MySqlParameter("@p_alcance", model.Alcance));
                    comando.Parameters.Add(new MySqlParameter("@p_esquivar", model.Esquivar ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_atacar", model.Atacar ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_interromper", model.Interromper ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_dano", model.Dano ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_oficios", model.Oficio ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_proficiencias", model.Proficiencia ?? string.Empty));

                    // Imagens
                    comando.Parameters.Add(new MySqlParameter("@p_image_name", imageName));
                    comando.Parameters.Add(new MySqlParameter("@p_image_data", imageData));

                    // Parâmetros JSON (Listas Relacionais)
                    comando.Parameters.Add(new MySqlParameter("@p_pericias", JsonSerializer.Serialize(model.Pericias.Select(x => new {
                        pericia = x.Pericia,
                        outros = x.Outros,
                        treinado = x.Treinado
                    }))));

                    comando.Parameters.Add(new MySqlParameter("@p_recursos_vitais", JsonSerializer.Serialize(model.Recursos.Select(x => new {
                        tipo_recurso = x.TipoRecurso, // Resolve a diferença entre TipoRecurso (C#) e tipo_recurso (MySQL)
                        maximo = x.Maximo,
                        atual = x.Atual
                    }))));

                    comando.Parameters.Add(new MySqlParameter("@p_atributos", JsonSerializer.Serialize(model.Atributos.Select(x => new {
                        atributo = x.Atributo,
                        pontos = x.Pontos
                    }))));

                    comando.Parameters.Add(new MySqlParameter("@p_ataques", JsonSerializer.Serialize(model.Ataques.Select(x => new {
                        categoria = x.Categoria,
                        tipo = x.Tipo,
                        bonus = x.Bonus,
                        extra = x.Extra
                    }))));

                    comando.Parameters.Add(new MySqlParameter("@p_emocoes", JsonSerializer.Serialize(model.Emocoes.Select(x => new {
                        emocao = x.Emocao,
                        ranking = x.Ranking
                    }))));

                    comando.Parameters.Add(new MySqlParameter("@p_sanidade", JsonSerializer.Serialize(model.Sanidades.Select(x => new {
                        situacao = x.Situacao,
                        nivel = x.Nivel
                    }))));

                    if (comando.Connection.State != System.Data.ConnectionState.Open)
                    {
                        await comando.Connection.OpenAsync();
                    }

                    await comando.ExecuteNonQueryAsync();

                }

                return Ok(new { message = "Ficha salva com sucesso!" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao atualizar a ficha: {ex.Message}");
            }
            
            return RedirectToAction("Agente", model.Id);
        }

        ////////////////////////////////////
        ///  Adição de Itens e Habilidade
        ///////////////////////////////////

        [HttpPost]
        [Route("Agentes/AdicionarItem/{idPersonagem}")]
        public async Task<IActionResult> AdicionarItem(int idPersonagem, [FromBody] ItensFormItem model)
        {
            if (idPersonagem <= 0 || string.IsNullOrEmpty(model.Item))
                return BadRequest("Dados inválidos ou Personagem não salvo.");

            try
            {
                using (var comando = _context.Database.GetDbConnection().CreateCommand())
                {
                    // Chamando a Stored Procedure de Item
                    comando.CommandText = "sp_add_item";
                    comando.CommandType = System.Data.CommandType.StoredProcedure;

                    comando.Parameters.Add(new MySqlParameter("@p_id_personagem", idPersonagem));
                    comando.Parameters.Add(new MySqlParameter("@p_tipo", model.Tipo));
                    comando.Parameters.Add(new MySqlParameter("@p_item", model.Item));
                    comando.Parameters.Add(new MySqlParameter("@p_qtd", model.Qtd));

                    if (comando.Connection.State != System.Data.ConnectionState.Open)
                        await comando.Connection.OpenAsync();

                    // Executa a Procedure
                    await comando.ExecuteNonQueryAsync();

                    // Pega o ID gerado pelo banco para devolver ao JavaScript
                    comando.CommandText = "SELECT LAST_INSERT_ID();";
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.Parameters.Clear();
                    var newId = Convert.ToInt32(await comando.ExecuteScalarAsync());

                    return Ok(new { id = newId, message = "Adicionado com sucesso" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao inserir item: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Agentes/AdicionarHabilidade/{idPersonagem}")]
        public async Task<IActionResult> AdicionarHabilidade(int idPersonagem, [FromBody] HabilidadeFormItem model)
        {
            if (idPersonagem <= 0 || string.IsNullOrEmpty(model.Habilidade))
                return BadRequest("Dados inválidos ou Personagem não salvo.");

            try
            {
                using (var comando = _context.Database.GetDbConnection().CreateCommand())
                {
                    // Chamando a Stored Procedure de Habilidade
                    comando.CommandText = "sp_add_hability";
                    comando.CommandType = System.Data.CommandType.StoredProcedure;

                    comando.Parameters.Add(new MySqlParameter("@p_id_personagem", idPersonagem));
                    comando.Parameters.Add(new MySqlParameter("@p_habilidade", model.Habilidade));
                    comando.Parameters.Add(new MySqlParameter("@p_custo", model.Custo));
                    comando.Parameters.Add(new MySqlParameter("@p_alcance", model.Alcance));
                    comando.Parameters.Add(new MySqlParameter("@p_aprimorar", model.Aprimorado));
                    comando.Parameters.Add(new MySqlParameter("@p_efeito", model.Efeito ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_arma", model.Arma));
                    comando.Parameters.Add(new MySqlParameter("@p_acao", model.Acao));

                    if (comando.Connection.State != System.Data.ConnectionState.Open)
                        await comando.Connection.OpenAsync();

                    // Executa a Procedure
                    await comando.ExecuteNonQueryAsync();

                    // Pega o ID gerado pelo banco para devolver ao JavaScript
                    comando.CommandText = "SELECT LAST_INSERT_ID();";
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.Parameters.Clear();
                    var newId = Convert.ToInt32(await comando.ExecuteScalarAsync());

                    return Ok(new { id = newId, message = "Adicionado com sucesso" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao inserir habilidade: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Agentes/EditarItem/{idItem}")]
        public async Task<IActionResult> EditarItem(int idItem, [FromBody] ItensFormItem model)
        {
            if (idItem <= 0 || string.IsNullOrEmpty(model.Item))
                return BadRequest("Dados de edição inválidos.");

            try
            {
                using (var comando = _context.Database.GetDbConnection().CreateCommand())
                {
                    comando.CommandText = "sp_edit_item";
                    comando.CommandType = System.Data.CommandType.StoredProcedure;

                    comando.Parameters.Add(new MySqlParameter("@p_id", idItem));
                    comando.Parameters.Add(new MySqlParameter("@p_tipo", model.Tipo));
                    comando.Parameters.Add(new MySqlParameter("@p_item", model.Item));
                    comando.Parameters.Add(new MySqlParameter("@p_qtd", model.Qtd));

                    if (comando.Connection.State != System.Data.ConnectionState.Open)
                        await comando.Connection.OpenAsync();

                    await comando.ExecuteNonQueryAsync();

                    return Ok(new { message = "Item atualizado com sucesso" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar item: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Agentes/EditarHabilidade/{idHabilidade}")]
        public async Task<IActionResult> EditarHabilidade(int idHabilidade, [FromBody] HabilidadeFormItem model)
        {
            if (idHabilidade <= 0 || string.IsNullOrEmpty(model.Habilidade))
                return BadRequest("Dados de edição inválidos.");

            try
            {
                using (var comando = _context.Database.GetDbConnection().CreateCommand())
                {
                    comando.CommandText = "sp_edit_hability";
                    comando.CommandType = System.Data.CommandType.StoredProcedure;

                    comando.Parameters.Add(new MySqlParameter("@p_id", idHabilidade));
                    comando.Parameters.Add(new MySqlParameter("@p_habilidade", model.Habilidade));
                    comando.Parameters.Add(new MySqlParameter("@p_custo", model.Custo));
                    comando.Parameters.Add(new MySqlParameter("@p_alcance", model.Alcance));
                    comando.Parameters.Add(new MySqlParameter("@p_aprimorar", model.Aprimorado));
                    comando.Parameters.Add(new MySqlParameter("@p_efeito", model.Efeito ?? string.Empty));
                    comando.Parameters.Add(new MySqlParameter("@p_arma", model.Arma));
                    comando.Parameters.Add(new MySqlParameter("@p_acao", model.Acao));

                    if (comando.Connection.State != System.Data.ConnectionState.Open)
                        await comando.Connection.OpenAsync();

                    await comando.ExecuteNonQueryAsync();

                    return Ok(new { message = "Habilidade atualizada com sucesso" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar habilidade: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("Agentes/DeletarItem/{idItem}")]
        public async Task<IActionResult> DeletarItem(int idItem)
        {
            if (idItem <= 0)
                return BadRequest("ID do item inválido.");

            try
            {
                using (var comando = _context.Database.GetDbConnection().CreateCommand())
                {
                    comando.CommandText = "sp_del_item";
                    comando.CommandType = System.Data.CommandType.StoredProcedure;

                    comando.Parameters.Add(new MySqlParameter("@p_id", idItem));

                    if (comando.Connection.State != System.Data.ConnectionState.Open)
                        await comando.Connection.OpenAsync();

                    await comando.ExecuteNonQueryAsync();

                    return Ok(new { message = "Item excluído com sucesso." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir item: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("Agentes/DeletarHabilidade/{idHabilidade}")]
        public async Task<IActionResult> DeletarHabilidade(int idHabilidade)
        {
            if (idHabilidade <= 0)
                return BadRequest("ID da habilidade inválido.");

            try
            {
                using (var comando = _context.Database.GetDbConnection().CreateCommand())
                {
                    comando.CommandText = "sp_del_hability";
                    comando.CommandType = System.Data.CommandType.StoredProcedure;

                    comando.Parameters.Add(new MySqlParameter("@p_id", idHabilidade));

                    if (comando.Connection.State != System.Data.ConnectionState.Open)
                        await comando.Connection.OpenAsync();

                    await comando.ExecuteNonQueryAsync();

                    return Ok(new { message = "Habilidade excluída com sucesso." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir habilidade: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("Agentes/DeletarAgente/{idPersonagem}")]
        public async Task<IActionResult> Deletar(int idPersonagem)
        {
            try
            {
                using (var comando = _context.Database.GetDbConnection().CreateCommand())
                {
                    comando.CommandText = "sp_deletar_personagem";
                    comando.CommandType = System.Data.CommandType.StoredProcedure;

                    comando.Transaction =
                        _context.Database.CurrentTransaction?.GetDbTransaction();

                    comando.Parameters.Add(
                        new MySqlParameter("@p_id_personagem", idPersonagem)
                    );

                    if (comando.Connection.State != System.Data.ConnectionState.Open)
                    {
                        await comando.Connection.OpenAsync();
                    }

                    await comando.ExecuteNonQueryAsync();
                }

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Personagem excluído com sucesso."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao excluir personagem: {ex.Message}"
                );
            }
        }

    }
}
