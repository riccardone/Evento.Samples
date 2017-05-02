using System;
using System.Collections.Generic;
using System.Linq;
using Evento.Domain.Aggregates.ValueObjects;
using Evento.Domain.Events;

namespace Evento.Domain.Aggregates
{
    public class AssociateAccount : AggregateBase
    {
        public override string AggregateId => CorrelationId.ToString();
        private Guid CorrelationId { get; set; }
        private List<Income> Incomes { get; }
        private List<Expense> Expenses { get; }

        public AssociateAccount(Guid correlationId, Guid associateId) : this()
        {
            RaiseEvent(new AssociateAccountCreated(correlationId, associateId));
        }

        public AssociateAccount()
        {
            Incomes = new List<Income>();
            Expenses = new List<Expense>();
            RegisterTransition<AssociateAccountCreated>(Apply);
            RegisterTransition<IncomeRegistered>(Apply);
            RegisterTransition<ExpenseRegistered>(Apply);
        }

        private void Apply(ExpenseRegistered evt)
        {
            Expenses.Add(new Expense(evt.Value, evt.Description));
        }

        private void Apply(IncomeRegistered evt)
        {
            Incomes.Add(new Income(evt.Value, evt.Description));
        }

        private void Apply(AssociateAccountCreated evt)
        {
            CorrelationId = evt.CorrelationId;
        }

        public static IAggregate Create(Guid correlationId, Guid associateId)
        {
            return new AssociateAccount(correlationId, associateId);
        }

        public void RegisterIncome(decimal value, string description)
        {
            var incomesTotal = Incomes.Select(a => a.Value).Sum() + value;
            var currentBalance = incomesTotal - Expenses.Select(a => a.Value).Sum();

            RaiseEvent(new IncomeRegistered(value, description, currentBalance));
        }

        public void RegisterExpense(decimal value, string description)
        {
            var expensesTotal = Expenses.Select(a => a.Value).Sum() + value;
            var currentBalance = Incomes.Select(a => a.Value).Sum() - expensesTotal;

            RaiseEvent(new ExpenseRegistered(value, description, currentBalance));
        }
    }
}
