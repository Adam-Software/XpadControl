using XpadControl.Interfaces.BindingButtonsService.Dependencies;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel
{
    public class ButtonActionBinding
    {
        public AdamActions Action { get; set; }
        public ConfigButtons Button { get; set; }
    }
}
