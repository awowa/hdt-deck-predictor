﻿using DeckPredictor;
using DeckPredictorTests.Mocks;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DeckPredictorTests.Tests
{
	[TestClass]
	public class PredictorTest
	{
		[TestMethod]
		public void OnGameStart_EmptyMetaDecks()
		{
			var predictor = new Predictor(new List<Deck>());
			predictor.OnGameStart(new MockGame());
			Assert.AreEqual(0, predictor.GetPossibleDecks().Count);
		}

		[TestMethod]
		public void OnGameStart_OneMetaDeckSameClass()
		{
			var metaDecks = new List<Deck>();
			metaDecks.Add(new Deck());
			metaDecks[0].Class = "Hunter";
			var predictor = new Predictor(metaDecks);

			var game = new MockGame();
			game.Opponent.Class = "Hunter";
			predictor.OnGameStart(game);

			Assert.IsTrue(metaDecks.SequenceEqual(predictor.GetPossibleDecks()));
		}

		[TestMethod]
		public void OnGameStart_OneMetaDeckDifferentClass()
		{
			var metaDecks = new List<Deck>();
			metaDecks.Add(new Deck());
			metaDecks[0].Class = "Hunter";
			var predictor = new Predictor(metaDecks);

			var game = new MockGame();
			game.Opponent.Class = "Mage";
			predictor.OnGameStart(game);

			Assert.AreEqual(0, predictor.GetPossibleDecks().Count);
		}

		[TestMethod]
		public void OnOpponentPlay_MissingCardFiltersDeck()
		{
			var metaDecks = new List<Deck>();
			metaDecks.Add(new Deck());
			metaDecks[0].Class = "Hunter";
			var predictor = new Predictor(metaDecks);

			var game = new MockGame();
			game.Opponent.Class = "Hunter";
			predictor.OnGameStart(game);
			game.AddOpponentCard("EX1_617", CardType.SPELL);
			predictor.OnOpponentPlay(null);

			Assert.AreEqual(0, predictor.GetPossibleDecks().Count);
		}

		[TestMethod]
		public void OnOpponentPlay_MatchingCardDoesNotFilter()
		{
			var metaDecks = new List<Deck>();
			metaDecks.Add(new Deck());
			metaDecks[0].Class = "Hunter";
			var hunterCard = new Card();
			hunterCard.Id = "EX1_617";
			metaDecks[0].Cards.Add(hunterCard);
			var predictor = new Predictor(metaDecks);

			var game = new MockGame();
			game.Opponent.Class = "Hunter";
			predictor.OnGameStart(game);
			game.AddOpponentCard("EX1_617", CardType.SPELL);
			predictor.OnOpponentPlay(null);

			Assert.AreEqual(1, predictor.GetPossibleDecks().Count);
		}

	}
}