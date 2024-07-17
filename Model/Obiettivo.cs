namespace Daidokoro.Model
{
    public class Obiettivo
    {
        public int IdObiettivo {  get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descrizione { get; set; } = string.Empty;
        public int Esperienza { get; set; }
        public int IdCategoria { get; set; }
        public DateTime DataOttenimento { get; set; }

        public string DataOttenimentoToString
        {
            get
            {
                return DataOttenimento.ToString("dd/MM/yyyy");
            }
        }
    }
}
