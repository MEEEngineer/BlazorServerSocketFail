using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorServerSocketFailTestApp.Pages
{
	public partial class LotsOfSockets
	{
		bool SockProcessRunning { get; set; } = false;
		int SocketsTried { get; set; }
		int SocketsOpen { get; set; }
		int SocketsFailed{ get; set; }

		string ErrorMessage { get; set; } = null;

		//This is totally the wrong way of using the HttpContexts, but it's done this way to keep sockets open for this example.
		List<HttpClient> ListOfContexts { get; set; }
		//Keep opening sockets until it starts getting an error:
		public async Task StartSocketBuild()
		{
			//Hide the button show the progress:
			this.SockProcessRunning = true;
			this.SocketsTried = 0;
			this.SocketsOpen = 0;
			this.SocketsFailed = 0;

			ListOfContexts = new List<HttpClient>();
			StateHasChanged();



			try
			{
				do
				{
					this.SocketsTried++;

					HttpClient httpClient = new HttpClient();

					httpClient.Timeout = TimeSpan.FromMinutes(20); //Keep that socket open!

					_ = httpClient.GetAsync("http://www.google.com");
					ListOfContexts.Add(httpClient);
					this.SocketsOpen++;

					await Task.Delay(1);

					StateHasChanged();
				} while (this.SocketsTried < 5000);
			}catch(Exception e)
			{
				this.ErrorMessage = e.Message;
			}


			this.SockProcessRunning = false;
			StateHasChanged();
		}
	}
}
