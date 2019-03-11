using System;
using System.ComponentModel.DataAnnotations;

namespace AppPinger.DB
{
    class LogModel
    {
        [Key]
        [Required(ErrorMessage = "Параметр IdLog не может быть пустым.")]
        public Guid IdLog { get; set; }
        public string NameProtocol { get; set; }
        public string DataLog { get; set; }
    }
}
