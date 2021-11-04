using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using MudBlazor;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ServiceReference1.TextCasingSoapTypeClient;

namespace BlazorServerSocketFailTestApp.Pages
{
	public partial class LotsOfSockets
	{

		//[Inject] ServiceReference1.TextCasingSoapType SoapService { get; set; }
		bool SockProcessRunning { get; set; } = false;
		int SocketsTried { get; set; }
		int SocketsOpen { get; set; }
		int SocketsFailed{ get; set; }

		string ErrorMessage { get; set; } = null;

		//This is totally the wrong way of using the HttpContexts, but it's done this way to keep sockets open for this example.
		List<HttpClient> ListOfContexts { get; set; }
		List<TextCasingSoapTypeClient> ListOfSoapClients { get; set; }
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

		public async Task StartSoapCalls()
		{
			//Hide the button show the progress:
			this.SockProcessRunning = true;
			this.SocketsTried = 0;
			this.SocketsOpen = 0;
			this.SocketsFailed = 0;

			ListOfSoapClients = new List<TextCasingSoapTypeClient>();
			StateHasChanged();



			try
			{
				do
				{
					this.SocketsTried++;
					EndpointConfiguration endpointConfiguration = EndpointConfiguration.TextCasingSoap;
					TextCasingSoapTypeClient testClient = new TextCasingSoapTypeClient(endpointConfiguration);

					_ = await testClient.AllLowercaseWithTokenAsync("SampleTest", "Token");

					ListOfSoapClients.Add(testClient);
					await Task.Delay(1);

					StateHasChanged();
				} while (this.SocketsTried < 5000);
			}
			catch (Exception e)
			{
				this.ErrorMessage = e.Message;
			}


			this.SockProcessRunning = false;
			StateHasChanged();
		}
	}
}
