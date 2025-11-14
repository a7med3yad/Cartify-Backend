using Cartify.Application.Contracts;
using Cartify.Application.Services.Interfaces;
using Cartify.Application.Services.Interfaces.Authentication;
using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Services.Implementation
{
	public class SubmitTicket : ISubmitTicket
	{
		private readonly IEmailSender _emailSender;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserService _userService;

		public SubmitTicket(IEmailSender emailSender , IUnitOfWork unitOfWork,IUserService userService)
		{
			_emailSender = emailSender;
			_unitOfWork = unitOfWork;
			_userService = userService;
		}
		public async Task<bool> SendTicket(SendTicketDto ticket)
		{
			var check = await _userService.GetByEmail(ticket.Email);
			if (check==null)
			{
				return false;
			}
			_emailSender.SendEmail(ticket.SenderName, ticket.SenderEmail,ticket.Name, ticket.Email,"support ticket", "We've Recived your ticket and we will respond as soon as we can");
			Ticket ticket1 = new Ticket { Email = ticket.Email,Name=ticket.Name,IssueCategory=ticket.IssueCategory,Message=ticket.Message,Subject=ticket.Subject };
			await _unitOfWork.TicketRepository.CreateAsync(ticket1);
			await _unitOfWork.SaveChanges();
			return true;

		}
	}
}
