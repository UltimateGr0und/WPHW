using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WPHW6.Models
{
    public class Quest
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required(ErrorMessage ="name is required")]
        [MaxLength(50,ErrorMessage ="too long name")]
        [Display(Name="Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "description is required")]
        [MaxLength(500, ErrorMessage = "too long description")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Time is required")]
        [Display(Name = "Time")]
        public double Time { get; set; }
        [Required(ErrorMessage = "Min players is required")]
        [Display(Name = "Minimum players")]
        public int MinPlayers { get; set; }
        [Required(ErrorMessage = "Max players is required")]
        [Display(Name = "Maximum players")]
        public int MaxPlayers { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [MaxLength(50, ErrorMessage = "too long address")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [MaxLength(20, ErrorMessage = "too long phone number")]
        [Display(Name = "Phone number")]
        public string Phone { get; set; }
        [MaxLength(50, ErrorMessage = "too long email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [MaxLength(50, ErrorMessage = "too long company name")]
        [Display(Name = "Company")]
        public string Company { get; set; }
        [Display(Name = "Rating")]
        [HiddenInput(DisplayValue = false)]
        public double Rating { get; set; }
        [Required(ErrorMessage = "FearLevel is required")]
        [Display(Name = "Fear level")]
        public double FearLevel { get; set; }
        [Required(ErrorMessage = "DifficultyLevel is required")]
        [Display(Name = "Difficulty level")]
        public double DifficultyLevel { get; set; }
        public string Logo { get; set; }
        public List<string> Gallery { get; set; }
    }
}