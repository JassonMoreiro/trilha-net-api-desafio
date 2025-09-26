using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // FEITO: Buscar o Id no banco utilizando o EF
            var tarefa = _context.Tarefas.Find(id);
            // FEITO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
           if (tarefa == null)
            {
                return NotFound();
            }
            return Ok(tarefa); // caso contrário retornar OK com a tarefa encontrada
            
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // FEITO: Buscar todas as tarefas no banco utilizando o EF
            var tarefa = _context.Tarefas.ToList();
            return Ok();
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // FEITO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            var tarefa = _context.Tarefas.Where(x => x.Titulo.Contains(titulo));
            return Ok(tarefa);
            // Dica: Usar como exemplo o endpoint ObterPorData
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // FEITO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            var tarefaBanco = _context.Tarefas.Add(tarefa);
            _context.SaveChanges();          
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // FEITO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            var tarefaAtualizada = tarefaBanco;
            tarefaAtualizada.Titulo = tarefa.Titulo;
            // FEITO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            var tarefaAtualizadaNoBanco = _context.Tarefas.Update(tarefaAtualizada);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            var tarefaRemovida = _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
