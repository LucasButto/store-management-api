﻿using System.Collections;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace store_management_api.Entities
{
    public class Ubicacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NameLocation { get; set; }
        public Boolean ExpDate { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuarios Usuario { get; set; }
        public int UsuarioId { get; set; }
        
        public ICollection<Producto> Productos { get; set; }
        public Ubicacion (string nameLocation, bool expDate)
        {
            NameLocation = nameLocation;
            ExpDate = expDate;
        }

        public Ubicacion(){ }

    }
}
