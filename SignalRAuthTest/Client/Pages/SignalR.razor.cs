using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRAuthTest.Client.Pages
{
    public partial class SignalR : ComponentBase
    {
        [Inject]
        IAccessTokenProvider tokenProvider { get; set; }
        private HubConnection hubConnection;
        private List<string> messages = new List<string>();
        private string userInput;
        private string messageInput;

        protected override async Task OnInitializedAsync()
        {
            var tokenResult = await tokenProvider.RequestAccessToken();
            if (tokenResult.TryGetToken(out var token))
            {
                hubConnection = new HubConnectionBuilder()
                .WithUrl(
                    NavigationManager.ToAbsoluteUri("/messageshub/?access_token={token.Value}")
                )
                .Build();

                hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    var encodedMsg = $"{user}: {message}";
                    messages.Add(encodedMsg);
                    StateHasChanged();
                });

                await hubConnection.StartAsync();
            }
        }

        public Task Send() =>
            hubConnection.SendAsync("SendMessage", userInput, messageInput);

        public bool IsConnected =>
            hubConnection != null ?
                hubConnection.State == HubConnectionState.Connected : false;

        public void Dispose()
        {
            _ = hubConnection.DisposeAsync();
        }
    }
}
