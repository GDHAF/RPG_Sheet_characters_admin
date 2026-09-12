namespace RPG_Sheet_characters_admin.Dto
{
    public class AtualizarPersonagemDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int NivelFis { get; set; }
        public int NivelPsi { get; set; }
        public string Origem { get; set; } = string.Empty;
        public int Idade { get; set; }
        public string Classe { get; set; } = string.Empty;
        public string Patente { get; set; } = string.Empty;

        // personagem_info
        public string Descricao { get; set; } = string.Empty;
        public string Historia { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string Armadura { get; set; } = string.Empty;
        public string Arma { get; set; } = string.Empty;
        public int DefArma { get; set; }
        public int Alcance { get; set; }
        public string Esquivar { get; set; } = string.Empty;
        public string Atacar { get; set; } = string.Empty;
        public string Interromper { get; set; } = string.Empty;
        public string Dano { get; set; } = string.Empty;
        public string Oficios { get; set; } = string.Empty;
        public string Proficiencias { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public byte[]? ImageData { get; set; }

        // Listas estruturadas que viram JSON
        public List<PericiaDto>? Pericias { get; set; }
        public List<RecursoVitalDto>? RecursosVitais { get; set; }
        public List<AtributoDto>? Atributos { get; set; }
        public List<AtaqueDto>? Ataques { get; set; }
        public List<EmocaoDto>? Emocoes { get; set; }
        public List<SanidadeDto>? Sanidade { get; set; }
    }
}
