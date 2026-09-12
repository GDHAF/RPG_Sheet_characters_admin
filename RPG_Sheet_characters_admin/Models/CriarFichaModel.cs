using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace RPG_Sheet_characters_admin.Models
{
    public class CriarFichaModel
    {
        public int? Id { get; set; } = 0;
        // Cabeçalho e Biografia
        public string Nome { get; set; } = string.Empty;
        public string? Jogador { get; set; } = string.Empty;
        public string? Cod_Agente {  get; set; } = string.Empty;
        public int? Idade { get; set; } = 0;
        public int? Nivel_Fis { get; set; } = 1;
        public int? Nivel_Psi { get; set; } = 1;
        public string? Image_Name {  get; set; } = string.Empty;
        public IFormFile? Imagem { get; set; } // Captura o arquivo do input tipo file
        public byte[]? Image_Data { get; set; }
        public string? Origem { get; set; } = string.Empty;
        public string? Classe { get; set; } = string.Empty;
        public string? Patente { get; set; } = "Carmesim";
        public string? Status {  get; set; } = "Ativo";
        public string? Equipe {  get; set; } = string.Empty;
        public string? Descricao { get; set; } = string.Empty;
        public string? Historia { get; set; } = string.Empty;
        public string? Notes { get; set; } = string.Empty;
        public string? Infracoes {  get; set; } = string.Empty;
        public string? Oficio { get; set; } = string.Empty;
        public string? Proficiencia { get; set; } = string.Empty;
        public string? Armadura {  get; set; } = string.Empty;
        public string? Arma { get; set; } = string.Empty;
        public int? Def_arma {  get; set; } = 0;
        public int? Alcance { get; set; } = 1;
        public string? Esquivar { get; set;  } = string.Empty;
        public string? Atacar { get; set; } = string.Empty;
        public string? Interromper { get; set; } = string.Empty;
        public string? Dano { get; set; } = string.Empty;

        // Atributos (Preenchidos automaticamente através de Atributos[0], Atributos[1]...)
        public List<AtributoFormItem> Atributos { get; set; } = new();
        public List<PericiaFormItem> Pericias { get; set; } = new();
        public List<RecursoFormItem> Recursos { get; set; } = new();
        public List<AtaquesFormItem> Ataques { get; set; } = new();
        public List<EmocoesFormItem> Emocoes { get; set; } = new();
        public List<SituacoesFormItem> Sanidades { get; set; } = new();
        public List<ItensFormItem> Itens { get; set; } = new();
        public List<HabilidadeFormItem> Habilidades { get; set; } = new();
    }

    public class AtributoFormItem
    {
        public string? Atributo { get; set; } = string.Empty;
        public int? Pontos { get; set; } = 10;
    }

    public class PericiaFormItem
    {
        public string? Pericia { get; set; } = string.Empty;
        public bool? Treinado { get; set; } = false;
        public int? Outros { get; set; } = 0;
    }

    public class RecursoFormItem
    {
        public string? TipoRecurso { get; set; } = string.Empty;
        public int? Maximo { get; set; } = 0;
        public int? Atual { get; set; } = 0;
    }

    public class AtaquesFormItem
    {
        public string? Categoria { get; set; } = string.Empty;
        public string? Tipo { get; set; } = string.Empty;
        public int? Bonus { get; set; } = 0;
        public string? Extra { get; set; } = string.Empty;
    }

    public class EmocoesFormItem
    {
        public string? Emocao { get; set; } = string.Empty;
        public string? Ranking { get; set; } = "F";
    }

    public class SituacoesFormItem
    {
        public string? Situacao { get; set; } = string.Empty;
        public string? Nivel { get; set; } = "B";
    }

    public class ItensFormItem
    {
        public int? Id { get; set; } = 0;
        public string? Tipo { get; set; } = string.Empty;
        public string? Item { get; set; } = string.Empty;
        public int? Qtd { get; set; } = 1;
    }

    public class HabilidadeFormItem
    {
        public int? Id { get; set; } = 0;
        public string? Habilidade { get; set; } = string.Empty;
        public int? Custo { get; set; } = 0;
        public int? Alcance { get; set; } = 0;
        public bool? Aprimorado { get; set; } = false;
        public string? Efeito { get; set; } = string.Empty;
        public bool? Arma { get; set; } = false;
        public bool? Acao { get; set; } = false;
    }
}
