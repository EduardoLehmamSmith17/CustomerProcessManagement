using CustomerProcessManagement.Models;
using CustomerProcessManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CustomerProcessManagement.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar operações relacionadas a pessoas físicas e jurídicas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IPhysicalPersonRepositorie _physicalPersonRepository;
        private readonly ILegalPersonRepositorie _legalPersonRepository;

        /// <summary>
        /// Construtor da classe PersonsController.
        /// </summary>
        /// <param name="physicalPersonRepository">Repositório de pessoas físicas.</param>
        /// <param name="legalPersonRepository">Repositório de pessoas jurídicas.</param>
        public PersonsController(IPhysicalPersonRepositorie physicalPersonRepository,
                                 ILegalPersonRepositorie legalPersonRepository)
        {
            _physicalPersonRepository = physicalPersonRepository;
            _legalPersonRepository = legalPersonRepository;
        }

        /// <summary>
        /// Obtém todas as pessoas cadastradas, filtradas por tipo (opcional).
        /// </summary>
        /// <param name="type">Tipo de pessoa a ser filtrada ("Pessoa física" ou "Pessoa jurídica").</param>
        /// <returns>Lista de todas as pessoas físicas e/ou jurídicas.</returns>
        [HttpGet]
        public async Task<ActionResult<List<object>>> GetAllPersons([FromQuery] string? type = null)
        {
            List<object> allPersons = new List<object>();

            if (type == null || type == "")
            {
                var physicalPersons = await _physicalPersonRepository.GetAllAsync();
                var legalPersons = await _legalPersonRepository.GetAllAsync();
                allPersons.AddRange(physicalPersons);
                allPersons.AddRange(legalPersons);
            }
            else
            {
                if (type.Trim().ToLower() == "pessoa física")
                {
                    var physicalPersons = await _physicalPersonRepository.GetAllAsync();
                    allPersons.AddRange(physicalPersons);
                }
                else if (type.Trim().ToLower() == "pessoa jurídica")
                {
                    var legalPersons = await _legalPersonRepository.GetAllAsync();
                    allPersons.AddRange(legalPersons);
                }
                else
                {
                    return BadRequest("Tipo de pessoa inválido. Use 'Pessoa física' ou 'Pessoa jurídica'.");
                }
            }

            return Ok(allPersons); ;
        }

        /// <summary>
        /// Obtém uma pessoa específica pelo ID.
        /// </summary>
        /// <param name="cpfOrCnpj">cpf/Cnpj da pessoa a ser obtida.</param>
        /// <returns>Pessoa física ou jurídica encontrada pelo ID.</returns>
        [HttpGet("{cpfOrCnpj}")]
        public async Task<ActionResult<object>> GetPerson(string cpfOrCnpj)
        {
            if (string.IsNullOrWhiteSpace(cpfOrCnpj))
            {
                return BadRequest(new { message = "CPF ou CNPJ é obrigatório." });
            }

            bool isCpf = cpfOrCnpj.Length == 11;
            bool isCnpj = cpfOrCnpj.Length == 14;

            if (!isCpf && !isCnpj)
            {
                return BadRequest(new { message = "CPF ou CNPJ inválido." });
            }

            if (isCpf)
            {
                var physicalPerson = await _physicalPersonRepository.GetAsync(cpfOrCnpj);
                if (physicalPerson != null)
                {
                    return Ok(physicalPerson);
                }
            }

            if (isCnpj)
            {
                var legalPerson = await _legalPersonRepository.GetAsync(cpfOrCnpj);
                if (legalPerson != null)
                {
                    return Ok(legalPerson);
                }
            }

            return NotFound(new { message = $"Pessoa com CPF/CNPJ: {cpfOrCnpj} não encontrada." });
        }

        /// <summary>
        /// Adiciona uma nova pessoa física ou jurídica.
        /// </summary>
        /// <param name="person">Dados da pessoa a ser adicionada.</param>
        /// <returns>Pessoa física ou jurídica adicionada com sucesso.</returns>
        [HttpPost]
        public async Task<ActionResult<object>> AddPerson([FromBody] JsonElement person)
        {
            try
            {
                if (person.ValueKind == JsonValueKind.Undefined || (person.ValueKind == JsonValueKind.Object && person.GetRawText() == "{}"))
                {
                    return BadRequest("Pessoa não inserida no banco");
                }

                if (person.TryGetProperty("idPhysicalPerson", out _))
                {
                    var physicalPerson = JsonSerializer.Deserialize<PhysicalPerson>(person.GetRawText(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (physicalPerson != null)
                    {
                        physicalPerson.CreatedDate = DateTime.UtcNow;
                        await _physicalPersonRepository.AddAsync(physicalPerson);

                        var physicalPersonInserrt = new PhysicalPerson
                        {
                            IdPhysicalPerson = physicalPerson.IdPhysicalPerson,
                            FullName = physicalPerson.FullName,
                            CPF = physicalPerson.CPF,
                            DateOfBirth = physicalPerson.DateOfBirth,
                            Address = physicalPerson.Address,
                            Contact =
                            {
                                Telephone = physicalPerson.Contact.Telephone,
                                Email = physicalPerson.Contact.Email
                            }
                        };

                        return Ok(physicalPersonInserrt);
                    }
                }
                else if (person.TryGetProperty("idLegalPerson", out _))
                {
                    var legalPerson = JsonSerializer.Deserialize<LegalPerson>(person.GetRawText(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (legalPerson != null)
                    {
                        legalPerson.CreatedDate = DateTime.UtcNow;
                        await _legalPersonRepository.AddAsync(legalPerson);

                        var legalPersonInsert = new LegalPerson
                        {
                            IdLegalPerson = legalPerson.IdLegalPerson,
                            CorporateReason = legalPerson.CorporateReason,
                            FantasyName = legalPerson.FantasyName,
                            CNPJ = legalPerson.CNPJ,
                            Address = legalPerson.Address,
                            Contact = new Contact
                            {
                                Telephone = legalPerson.Contact.Telephone,
                                Email = legalPerson.Contact.Email
                            }
                        };

                        return Ok(legalPersonInsert);
                    }
                }

                return BadRequest("Pessoa não inserida no banco");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao inserir pessoa: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza os dados de uma pessoa física ou jurídica pelo ID.
        /// </summary>
        /// <param name="cpf">cpf/Cnpj da pessoa a ser atualizada.</param>
        /// <param name="person">Novos dados da pessoa a serem atualizados.</param>
        /// <returns>Pessoa física ou jurídica atualizada com sucesso.</returns>
        [HttpPut("physical/{cpf}")]
        public async Task<ActionResult<object>> UpdatePhysicalPerson(string cpf, PhysicalPerson person)
        {
            try
            {
                if (person == null)
                {
                    return BadRequest("Dados inválidos. Pessoa não atualizada no banco.");
                }

                if (cpf.Length != 11)
                {
                    return BadRequest("O CPF informado não corresponde ao CPF da pessoa.");
                }

                var updatedPhysicalPerson = await _physicalPersonRepository.UpdateAsync(person, cpf);

                return Ok(updatedPhysicalPerson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar pessoa física: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza os dados de uma pessoa física ou jurídica pelo ID.
        /// </summary>
        /// <param name="cnpj">cpf/Cnpj da pessoa a ser atualizada.</param>
        /// <param name="person">Novos dados da pessoa a serem atualizados.</param>
        /// <returns>Pessoa física ou jurídica atualizada com sucesso.</returns>
        [HttpPut("legal/{cnpj}")]
        public async Task<ActionResult<object>> UpdateLegalPerson(string cnpj, LegalPerson person)
        {
            try
            {
                if (person == null)
                {
                    return BadRequest("Dados inválidos. Pessoa não atualizada no banco.");
                }

                if (cnpj.Length != 14)
                {
                    return BadRequest("O CNPJ informado não corresponde ao CNPJ da pessoa.");
                }

                person.UpdatedDate = DateTime.UtcNow;

                var updatedLegalPerson = await _legalPersonRepository.UpdateAsync(person, cnpj);

                return Ok(updatedLegalPerson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar pessoa jurídica: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove uma pessoa física ou jurídica pelo ID.
        /// </summary>
        /// <param name="id">ID da pessoa a ser removida.</param>
        /// <returns>Status 200 OK com mensagem indicando que a pessoa foi removida.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(string id)
        {
            try
            {
                var physicalPerson = await _physicalPersonRepository.GetAsync(id);
                if (physicalPerson != null)
                {
                    await _physicalPersonRepository.DeletePhysicalPersonAsync(physicalPerson.CPF!);
                    return Ok(new { message = "Pessoa física removida com sucesso." });
                }

                var legalPerson = await _legalPersonRepository.GetAsync(id);
                if (legalPerson != null)
                {
                    await _legalPersonRepository.DeleteLegalPersonAsync(legalPerson.CNPJ!);
                    return Ok(new { message = "Pessoa jurídica removida com sucesso." });
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar pessoa: {ex.Message}");
            }
        }
    }
}
