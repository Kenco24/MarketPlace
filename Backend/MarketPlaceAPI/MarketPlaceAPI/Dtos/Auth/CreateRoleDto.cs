﻿using System.ComponentModel.DataAnnotations;

namespace MarketPlaceAPI.Dtos.Auth
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role Name is required.")]
        public string RoleName { get; set; } = null!;
    }
}