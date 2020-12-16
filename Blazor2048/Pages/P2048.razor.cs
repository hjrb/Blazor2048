using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

partial class P2048 : ComponentBase {
    protected override async Task OnInitializedAsync()
    {
        await InvokeAsync(StateHasChanged);
    }
}