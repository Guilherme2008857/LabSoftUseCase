using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppTask.Models;

public partial class Funcionario
{
    [Key]
    public int Codigo { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; } = null!;

    [Required(ErrorMessage = "O cargo é obrigatório.")]
    [StringLength(50)]
    public string Cargo { get; set; } = null!;

    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}
