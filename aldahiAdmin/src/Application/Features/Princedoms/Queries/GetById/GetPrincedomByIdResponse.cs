using System;
namespace FirstCall.Application.Features.Princedoms.Queries.GetById
{
    public class GetPrincedomByIdResponse
    {
		
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
		
		public string ar_title { get; set; }
		public string en_title { get; set; }
		public string Code { get; set; }

		//public decimal Tax { get; set; }
    }
}