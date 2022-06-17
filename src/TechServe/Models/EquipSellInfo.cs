namespace TechServe.Models
{
    public class EquipSellInfo
    {
        public string id;
        public string name;
        public string producer;
        public string model;
        public string sell_date;
        public bool guarantee { get; set; }
        public string guarantee_name
        {
            get
            {
                return guarantee ? "Да" : "Нет";
            }
        }
        public string NameId { get; set; }
        public string EquipId { get; set; }
    }
}
