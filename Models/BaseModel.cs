﻿using System.ComponentModel.DataAnnotations;

namespace GwanjaLoveProto.Models
{
	public class BaseModel
	{
		public int Id { get; set; }
		[Required]
		public required string Name { get; set; }
		[Required]
		public required string Description { get; set; }
		public bool Active { get; set; }
	}
}
