namespace Daidokoro.Model
{
    public class Utente
    {
        public int IdUtente { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] Foto { get; set; } = null!;
        public string Pwd { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Esperienza {  get; set; }
        public int Livello { get; set; }
        
        public long Likes { get; set; }
        public long ReviewCount {  get; set; }
        public long RecipeCount {  get; set; }
        public long AchievementsCount { get; set; }
    }
}
