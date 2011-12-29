// Copyright (C) 2011, Mehdi Khalili
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.Atm
{
    [Story(
        AsA = "As an Account Holder",
        IWant = "I want to withdraw cash from an ATM",
        SoThat = "So that I can get money when the bank is closed")]
    [TestFixture]
    public class AccountHolderWithdrawsCash
    {
        private const string GivenTheAccountBalanceIsTitleTemplate = "Given the account balance is ${0}";
        private const string AndTheMachineContainsEnoughMoneyTitleTemplate = "And the machine contains enough money";
        private const string WhenTheAccountHolderRequestsTitleTemplate = "When the account holder requests ${0}";
        private const string AndTheCardShouldBeReturnedTitleTemplate = "And the card should be returned";

        private Card _card;
        private Atm _atm;

        public void GivenTheAccountBalanceIs(int balance)
        {
            _card = new Card(true, balance);
        }

        public void GivenTheCardIsDisabled()
        {
            _card = new Card(false, 100);
            _atm = new Atm(100);
        }

        public void AndTheCardIsValid()
        {
        }

        public void AndTheMachineContains(int atmBalance)
        {
            _atm = new Atm(atmBalance);
        }

        public void WhenTheAccountHolderRequests(int moneyRequest)
        {
            _atm.RequestMoney(_card, moneyRequest);
        }

        public void TheAtmShouldDispense(int dispensedMoney)
        {
            Assert.AreEqual(dispensedMoney, _atm.DispenseValue);
        }

        public void AndTheAccountBalanceShouldBe(int balance)
        {
            Assert.AreEqual(balance, _card.AccountBalance);
        }

        public void ThenCardIsRetained(bool cardIsRetained)
        {
            Assert.AreEqual(cardIsRetained, _atm.CardIsRetained);
        }

        void AndTheAtmShouldSayTheCardHasBeenRetained()
        {
            Assert.AreEqual(DisplayMessage.CardIsRetained, _atm.Message);
        }

        [Test]
        public void AccountHasInsufficientFund()
        {
            new AccountHasInsufficientFund().Bddify<AccountHolderWithdrawsCash>();
        }

        [Test]
        public void AccountHasSufficientFund()
        {
           this.Given(s => s.GivenTheAccountBalanceIs(100), GivenTheAccountBalanceIsTitleTemplate)
                    .And(s => s.AndTheCardIsValid())
                    .And(s => s.AndTheMachineContains(100), AndTheMachineContainsEnoughMoneyTitleTemplate)
                .When(s => s.WhenTheAccountHolderRequests(20), WhenTheAccountHolderRequestsTitleTemplate)
                .Then(s => s.TheAtmShouldDispense(20), "Then the ATM should dispense $20")
                    .And(s => s.AndTheAccountBalanceShouldBe(80), "And the account balance should be $80")
                    .And(s => s.ThenCardIsRetained(false), AndTheCardShouldBeReturnedTitleTemplate)
                .Bddify();
        }

        [Test]
        public void CardHasBeenDisabled()
        {
            this.Given(s => s.GivenTheCardIsDisabled())
                .When(s => s.WhenTheAccountHolderRequests(20))
                .Then(s => s.ThenCardIsRetained(true), false) // in here I am telling the fluent API that I do not want it to include the input arguments in the step title
                    .And(s => s.AndTheAtmShouldSayTheCardHasBeenRetained())
                .Bddify();
        }
    }
}