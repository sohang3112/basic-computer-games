import java.util.ArrayList;
import java.util.Arrays;
import java.util.LinkedList;
import java.util.List;

public class Game {
    
    private Deck deck;
    private UserIo userIo;

    public Game(Deck deck, UserIo userIo) {
        this.deck = deck;
        this.userIo = userIo;
    }

	/**
	 * Run the game, running rounds until ended with CTRL+D/CTRL+Z or CTRL+C
	 */
    public void run() {
		userIo.println("BLACK JACK", 31);
		userIo.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n", 15);
		if(userIo.promptBoolean("DO YOU WANT INSTRUCTIONS")){
			userIo.println("THIS IS THE GAME OF 21. AS MANY AS 7 PLAYERS MAY PLAY THE");
			userIo.println("GAME. ON EACH DEAL, BETS WILL BE ASKED FOR, AND THE");
			userIo.println("PLAYERS' BETS SHOULD BE TYPED IN. THE CARDS WILL THEN BE");
			userIo.println("DEALT, AND EACH PLAYER IN TURN PLAYS HIS HAND. THE");
			userIo.println("FIRST RESPONSE SHOULD BE EITHER 'D', INDICATING THAT THE");
			userIo.println("PLAYER IS DOUBLING DOWN, 'S', INDICATING THAT HE IS");
			userIo.println("STANDING, 'H', INDICATING HE WANTS ANOTHER CARD, OR '/',");
			userIo.println("INDICATING THAT HE WANTS TO SPLIT HIS CARDS. AFTER THE");
			userIo.println("INITIAL RESPONSE, ALL FURTHER RESPONSES SHOULD BE 'S' OR");
			userIo.println("'H', UNLESS THE CARDS WERE SPLIT, IN WHICH CASE DOUBLING");
			userIo.println("DOWN IS AGAIN PERMITTED. IN ORDER TO COLLECT FOR");
			userIo.println("BLACKJACK, THE INITIAL RESPONSE SHOULD BE 'S'.");
		}

		int nPlayers = 0;
		while(nPlayers < 1 || nPlayers > 7) {
			nPlayers = userIo.promptInt("NUMBER OF PLAYERS");
		}

		deck.reshuffle();

		List<Player> players = new ArrayList<>();
		for(int i = 0; i < nPlayers; i++) {
			players.add(new Player(i + 1));
		}

		while(true) {
			double[] bets = new double[nPlayers]; // empty array initialized with all '0' valuses.
			while(!betsAreValid(bets)){
				userIo.println("BETS:");
				for(int i = 0; i < nPlayers; i++) {
					// Note that the bet for player "1" is at index "0" in the bets
					// array and take care to avoid off-by-one errors.
					// TODO the BASIC version allows fractional bets (e.g. '1.5') of arbitrary precision
					//   We can use a double if we want--the original basic has doubleing point errors. Or we could use BigDecimal if we really want to fix that.
					bets[i] = userIo.promptDouble("#" + (i + 1));  //TODO: If there isn't a need for a separate Bets in the future, combine these two lines and convert to enhanced FOR loop
					players.get(i).setCurrentBet(bets[i]);
				}
			}

			for(Player player : players){
				player.dealCard(deck.deal());
				player.dealCard(deck.deal()); //TODO: This could be in a separate loop to more acurrately follow how a game would be dealt, I couldn't figure out of the BASIC version did it
			}


			// Consider adding a Dealer class to track the dealer's hand and running total.
			// Alternately, the dealer could just be a Player instance where currentBet=0 and is ignored.
			LinkedList<Card> dealerHand = new LinkedList<>();
			Player dealer = new Player(0); //Dealer is Player 0 - this can be converted into a dealer class later on
			dealer.dealCard(deck.deal());
			// TODO deal two cards to the dealer

			// TODO handle 'insurance' if the dealer's card is an Ace.

			printInitialDeal(players, dealer);

			// TODO if dealer has an ACE, prompt "ANY INSURANCE"
			//   if yes, print "INSURANCE BETS" and prompt each player with "# [x] ? " where X is player number
			//   insurance bets must be equal or less than half the player's regular bet

			// TODO check for dealer blackjack
			//   if blackjack, print "DEALER HAS A [x] IN THE HOLE\nFOR BLACKJACK" and skip to evaluateRound
			//     pay 2x insurance bets (insurance bet of 5 pays 10) if applicable
			//   if not, print "NO DEALER BLACKJACK"
			//     collect insurance bets if applicable

			for(Player player : players){
				play(player);
			}

			// only play the dealer if at least one player has not busted or gotten a natural blackjack (21 in the first two cards)
			// otherwise, just print the dealer's concealed card
			dealerHand = playDealer(dealerHand, deck);

			evaluateRound(players, dealerHand);
		}
    }

	/**
	 * Print the cards for each player and the up card for the dealer.
	 */
	private void printInitialDeal(List<Player> players, Player dealer) {
		// Prints the initial deal in the following format:
		/*
		PLAYER 1     2    DEALER
               7    10     4   
               2     A   
	   */
		
        StringBuilder output = new StringBuilder(); 
		output.append("PLAYERS ");
		for (Player player : players) {
			output.append(player.getPlayerNumber() + "\t");
		}
		output.append("DEALER\n");
		//Loop through two rows of cards		
        for (int j = 0; j < 2; j++) {
			output.append("\t");
			for (Player player : players) {
				output.append(player.getHand().get(j).toString()).append("\t");
			}
			if(j == 0 ){
				output.append(dealer.getHand().get(j).toString());
			}
			output.append("\n");
		}
		userIo.print(output.toString());
	}

	/**
	 * Plays the players turn. Prompts the user to hit (H), stay (S), or if
	 * appropriate, split (/) or double down (D), and then performs those
	 * actions. On a hit, prints "RECEIVED A  [x]  HIT? "
	 * 
	 * @param player
	 */
	protected void play(Player player) {
		play(player, 1);
	}

	private void play(Player player, int handNumber) {
		String action;
		if(player.isSplit()){
			action = userIo.prompt("HAND #" + handNumber);
		} else {
			action = userIo.prompt("PLAYER " + player.getPlayerNumber() + " ");
		}
		while(true){
			if(action.equalsIgnoreCase("H")){ // HIT
				Card c = deck.deal();
				player.dealCard(c, handNumber);
				if(scoreHand(player.getHand(handNumber)) > 21){
					userIo.println("...BUSTED");
					break;
				}
				action = userIo.prompt("RECEIVED A " + c.toString() + " HIT");
			} else if(action.equalsIgnoreCase("S")){ // STAY
				return;
			} else if(action.equalsIgnoreCase("D") && player.canDoubleDown(handNumber)) { // DOUBLE DOWN
				Card c = deck.deal();
				player.doubleDown(c, handNumber);
				if(scoreHand(player.getHand(handNumber)) > 21){
					userIo.println("...BUSTED");
					break;
				}
				userIo.println("RECEIVED A " + c.toString());
				break;
			} else if(action.equalsIgnoreCase("/")) { // SPLIT
				if(player.isSplit()) {
					// The original basic code printed different output
					// if a player tries to split twice vs if they try to split
					// a non-pair hand.
					action = userIo.prompt("TYPE H, S OR D, PLEASE");
				} else if(player.canSplit()) {
					player.split();
					Card card = deck.deal();
					player.dealCard(card, 1);
					// TODO move the "a" vs "an" logic to userIo or Card 
					if(card.getValue() == 1 || card.getValue() == 8) {
						userIo.println("FIRST HAND RECEIVES AN " + card.toString());
					} else {
						userIo.println("FIRST HAND RECEIVES A " + card.toString());
					}
					play(player, 1);
					card = deck.deal();
					player.dealCard(card, 2);
					if(card.getValue() == 1 || card.getValue() == 8) {
						userIo.println("SECOND HAND RECEIVES AN " + card.toString());
					} else {
						userIo.println("SECOND HAND RECEIVES A " + card.toString());
					}
					play(player, 2);
					break;
				} else {
					userIo.println("SPLITTING NOT ALLOWED");
					action = userIo.prompt("PLAYER " + player.getPlayerNumber() + " ");
				}
			} else {
				if(player.getHand(handNumber).size() == 2) {
					action = userIo.prompt("TYPE H,S,D, OR /, PLEASE");
				} else {
					action = userIo.prompt("TYPE H, OR S, PLEASE");
				}
			}
		}

	}

	/**
	 * Calculates the value of a hand. When the hand contains aces, it will
	 * count one of them as 11 if that does not result in a bust.
	 * 
	 * @param hand the hand to evaluate
	 * @return The numeric value of a hand. A value over 21 indicates a bust.
	 */
	protected int scoreHand(List<Card> hand){
		int nAces = (int) hand.stream().filter(c -> c.getValue() == 1).count();
		int value = hand.stream()
			.mapToInt(Card::getValue)
			.filter(v -> v != 1) // start without aces
			.map(v -> v > 10 ? 10 : v) // all face cards are worth 10. The 'expr ? a : b' syntax is called the 'ternary operator'
			.sum();
		value += nAces; // start by treating all aces as 1
		if(nAces > 0 && value <= 11) {
			value += 10; // We can use one of the aces to an 11
			// You can never use more than one ace as 11, since that would be 22 and a bust.
		}
		return value;
	}

	/**
	 * Compares two hands accounting for natural blackjacks using the 
	 * java.lang.Comparable convention of returning positive or negative integers
	 * 
	 * @param handA hand to compare
	 * @param handB other hand to compare
	 * @return a negative integer, zero, or a positive integer as handA is less than, equal to, or greater than handB.
	 */
	protected int compareHands(List<Card> handA, List<Card> handB) {
		int scoreA = this.scoreHand(handA);
		int scoreB = this.scoreHand(handB);

		if(scoreA == 21 && scoreB == 21){
			if(handA.size() == 2 && handB.size() != 2){
				return 1; //Hand A wins with a natural blackjack
			} else if (handA.size() != 2 && handB.size() == 2) {				
				return -1; //Hand B wins with a natural blackjack
			} else {
				return 0; //Tie
			}
		} else {
			return Integer.compare(scoreA, scoreB); 
		}
	}

	/**
	 * Play the dealer's hand. The dealer draws until they have >=17 or busts. Prints each draw as in the following example:
	 * 
	 * DEALER HAS A  5 CONCEALED FOR A TOTAL OF 11 
	 * DRAWS 10   ---TOTAL IS 21
	 * 
	 * TODO find out if the dealer draws on a "soft" 17 (17 using an ace as 11) or not in the original basic code.
	 * 
	 * @param dealerHand
	 * @return
	 */
	private LinkedList<Card> playDealer(LinkedList<Card> dealerHand, Deck deck) {
		// TODO implement playDealer
		return null;
	}

	/**
	 * Evaluates the result of the round, prints the results, and updates player/dealer totals.
	 * @param players
	 * @param dealerHand
	 */
	private void evaluateRound(List<Player> players, LinkedList<Card> dealerHand) {
		// TODO implement evaluateRound
		//   print something like:
		/*
		PLAYER 1 LOSES   100 TOTAL=-100 
		PLAYER 2  WINS   150 TOTAL= 150
		DEALER'S TOTAL= 200
		*/
		// this should probably take in a "Dealer" instance instead of just the dealer hand so we can update the dealer's total.
		// currentBets of each player are added/subtracted from the dealer total depending on whether they win/lose (accounting for doubling down, insurance etc.)
		// remember to handle a "PUSH" when the dealer ties and the bet is returned.
	}

	/**
	 * Validates that all bets are between 0 (exclusive) and 500 (inclusive). Fractional bets are valid.
	 * 
	 * @param bets The array of bets for each player.
	 * @return true if all bets are valid, false otherwise.
	 */
	public boolean betsAreValid(double[] bets) {
		return Arrays.stream(bets)
			.allMatch(bet -> bet > 0 && bet <= 500);
	}

}
