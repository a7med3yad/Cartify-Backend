using AutoMapper;
using Cartify.API.Contracts;
using Cartify.Application.Contracts;
using Cartify.Application.Contracts.HelpPageDtos;
using Cartify.Application.Services.Interfaces;
using Cartify.Application.Services.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cartify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpPageController : ControllerBase
    {
		private readonly ISubmitTicket _submitTicket;
		private readonly IMapper _mapper;

		public HelpPageController( ISubmitTicket submitTicket,IMapper mapper)
		{
			_submitTicket = submitTicket;
			_mapper = mapper;
		}
		[HttpPost]
		public async Task<IActionResult> SubmitTicket([FromBody] DtoSubmitTicket dto)
		{
			SendTicketDto dto2=new SendTicketDto { Email = dto.Email,IssueCategory=dto.IssueCategory,Message=dto.Message,Name=dto.Name,Subject=dto.Subject };
			var a=await _submitTicket.SendTicket(dto2);
			if (a)
			{
				return Ok();	
			}
			return NotFound();
		}
    }
}
