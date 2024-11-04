﻿using shoppingcart.Repositories.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shoppingcart.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
		public string Name { get; set; }
		public string Image {  get; set; }
		public string Slug { get; set; }

		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
		public string Description { get; set; }

		[Required( ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
		[Range(0.01,double.MaxValue)]
		[Column(TypeName = "decimal(8, 2)")]
		public decimal Price { get; set; }
		[Required,Range(1, int.MaxValue, ErrorMessage = "Yêu cầu chọn một thương hiệu")]
		public int BrandId { get; set; }
        [Required,Range(1, int.MaxValue, ErrorMessage = "Yêu cầu chọn một danh mục")]
        public int CategoryId { get; set; }	
		public CategoryModel Category { get; set; }	
		public BrandModel Brand { get; set; }

        [NotMapped]
		[FileExtension]
        //[Required(ErrorMessage = "Yêu cầu upload hình ảnh sản phẩm")]
        public IFormFile ImageUpload { get; set; }
	}
}
