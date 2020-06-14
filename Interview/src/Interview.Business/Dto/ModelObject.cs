using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interview.Business.Dto
{
	public class ModelObject : ModelObjectInput, IEntity
	{
		public int Id { get; set; }
	}
}
