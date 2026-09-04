using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppTask.Models;

public partial class Tarefa
{
    [Key]
    public int Codigo { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(200)]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = null!;

    [Required(ErrorMessage = "A data planejada é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data Planejada")]
    public DateTime DataPlanejada { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data Iniciada")]
    public DateTime? DataIniciada { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data Finalizada")]
    public DateTime? DataFinalizada { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data Cancelada")]
    public DateTime? DataCancelada { get; set; }

    [Required]
    [Display(Name = "Status")]
    public string StatusTarefa { get; set; } = null!;

    [Required]
    public string Prazo { get; set; } = null!;

    [Display(Name = "Funcionário Responsável")]
    public int CodigoFuncionario { get; set; }

    [ForeignKey("CodigoFuncionario")]
    [Display(Name = "Funcionário")]
    public virtual Funcionario? Funcionario { get; set; }
}
