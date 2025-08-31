namespace FirstCall.Application.Features.Nations.Queries.GetById
{
    public class GetNationByIdResponse
    {
		
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
		
		public string ArabicName { get; set; }
		public string Code { get; set; }
		public string PhoneCode { get; set; }

		//public decimal Tax { get; set; }
    }
}