using System;
using System.Collections.Generic;
using Zalandu.Shared;

namespace Zalandu.Client.Services
{
    public class StateContainerService
    {
        public Product      SelectProduct            { get; private set; }
        public List<String> SelectListOfClicksBySize { get; set; }
        public int          NumberOfItemsInCard      { get; set; }

        public event Action OnChange;

        public void SaveProduct(Product clothes)
        {
            this.SelectProduct = clothes;
            NotifyStateChanged();
        }

        public void SaveListOfClicksBySize(List<String> sizes)
        {
            this.SelectListOfClicksBySize = sizes;
            NotifyStateChanged();
        }

        public void SaveNumberOfItemsInCard(int amount)
        {
            this.NumberOfItemsInCard = amount;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
