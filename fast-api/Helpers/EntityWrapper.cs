using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using fast_api.EntityFramework.Entities;
using fast_api.Enums;

namespace fast_api.Helpers
{
    public class EntityWrapper
    {
        public EntityWrapper(Container container)
        {
            Id = GetWrapperId(ItemType, container.ContainerId);
            ItemType = ItemType.Container;
            Container = container;
        }

        public EntityWrapper(CurrencyTrade currencyTrade)
        {
            Id = GetWrapperId(ItemType, currencyTrade.CurrencyTradeId);
            ItemType = ItemType.Currency;
            CurrencyTrade = currencyTrade;
        }

        public EntityWrapper(SelectionContainer selectionContainer)
        {
            Id = GetWrapperId(ItemType, selectionContainer.SelectionContainerId);
            ItemType = ItemType.SelectionContainer;
            SelectionContainer = selectionContainer;
        }

        public ItemType ItemType { get; set; }
        public Container Container { get; set; }
        public CurrencyTrade CurrencyTrade { get; set; }
        public SelectionContainer SelectionContainer { get; set; }
        public EntityWrapper[] Dependencies { get; set; }
        public int Id { get; set; }

        public void CalculateDependencies(List<EntityWrapper> entities, ILookup<string, int> currencyTradeIdsByCurrency)
        {
            var dependencies = new List<EntityWrapper>();

            switch (ItemType)
            {
                case ItemType.SelectionContainer:
                    dependencies.AddRange(SelectionContainer.SelectionContainerCurrencies.SelectMany(x =>
                        currencyTradeIdsByCurrency[x.Currency]
                            .Select(id => entities.FirstOrDefault(entity => entity.Id == GetWrapperId(ItemType.Currency, id)))));
                    dependencies.AddRange(SelectionContainer.SelectionContainerContainers.Select(x =>
                        entities.FirstOrDefault(entity => entity.Id == GetWrapperId(ItemType.Container, x.ContainerId))));
                    break;
                case ItemType.Container:
                    dependencies.AddRange(Container.ContainerCurrencies.SelectMany(x =>
                        currencyTradeIdsByCurrency[x.Currency]
                            .Select(id => entities.FirstOrDefault(entity => entity.Id == GetWrapperId(ItemType.Currency, id)))));
                    dependencies.AddRange(Container.ContainerContainers.Select(x =>
                        entities.FirstOrDefault(entity => entity.Id == GetWrapperId(ItemType.Container, x.ContainerId))));
                    dependencies.AddRange(Container.ContainerSelectionContainers.Select(x =>
                        entities.FirstOrDefault(entity => entity.Id == GetWrapperId(ItemType.SelectionContainer, x.SelectionContainerId))));
                    break;
                case ItemType.Currency:
                    switch (CurrencyTrade.ItemType)
                    {
                        case ItemType.SelectionContainer:
                            dependencies.Add(entities.FirstOrDefault(x => x.Id == GetWrapperId(ItemType.SelectionContainer, CurrencyTrade.SelectionContainerId.Value)));
                            break;
                        case ItemType.Container:
                            dependencies.Add(entities.FirstOrDefault(x => x.Id == GetWrapperId(ItemType.Container, CurrencyTrade.ContainerId.Value)));
                            break;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Dependencies = dependencies.ToArray();
        }

        public static int GetWrapperId(ItemType itemType, int id) => int.Parse($"{itemType}{id}");
    }
}