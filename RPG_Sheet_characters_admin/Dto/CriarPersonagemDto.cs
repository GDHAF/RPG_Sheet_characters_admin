namespace RPG_Sheet_characters_admin.Dto
{
    public class CriarPersonagemDto
    {
        public string Jogador { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Origem { get; set; } = string.Empty;
        public int Idade { get; set; }
        public string Classe { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Historia { get; set; } = string.Empty;
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
        public byte[]? ImageData { get; set; } // Mapeia para LONGBLOB

        // Listas que serão convertidas em JSON
        public List<PericiaDto>? Pericias { get; set; }
        public List<RecursoVitalDto>? RecursosVitais { get; set; }
        public List<AtributoDto>? Atributos { get; set; }
        public List<AtaqueDto>? Ataques { get; set; }
        public List<EmocaoDto>? Emocoes { get; set; }
        public List<SanidadeDto>? Sanidade { get; set; }
    }

    public class PericiaDto { public string Pericia { get; set; } = string.Empty; public int Outros { get; set; } public bool Treinado { get; set; } }
    public class RecursoVitalDto { public string TipoRecurso { get; set; } = string.Empty; public int Maximo { get; set; } public int Atual { get; set; } }
    public class AtributoDto { public string Atributo { get; set; } = string.Empty; public int Pontos { get; set; } }
    public class AtaqueDto { public string Categoria { get; set; } = string.Empty; public string Tipo { get; set; } = string.Empty; public int Bonus { get; set; } public string Extra { get; set; } = string.Empty; }
    public class EmocaoDto { public string Emocao { get; set; } = string.Empty; public string Ranking { get; set; } = string.Empty; }
    public class SanidadeDto { public string Situacao { get; set; } = string.Empty; public string Nivel { get; set; } = string.Empty; }
}
