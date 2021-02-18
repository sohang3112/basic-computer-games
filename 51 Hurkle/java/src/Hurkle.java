import java.util.ArrayList;
import java.util.Scanner;

public class Hurkle {

    public static final int GRID_SIZE = 10;
    public static final int MAX_GUESSES = 5;

    private enum GAME_STATE {
        STARTING,
        START_GAME,
        GUESSING,
        PLAY_AGAIN,
        GAME_OVER
    }

    private GAME_STATE gameState;

    // Used for keyboard input
    private Scanner kbScanner;

    private int guesses;

    // hurkle position
    private int hurkleXPos;
    private int hurkleYPos;

    // player guess
    private int playerGuessXPos;
    private int playerGuessYPos;

    public Hurkle() {

        gameState = GAME_STATE.STARTING;

        // Initialise kb scanner
        kbScanner = new Scanner(System.in);
    }

    /**
     * Main game loop
     */
    public void play() {

        do {
            switch (gameState) {

                // Show an introduction the first time the game is played.
                case STARTING:
                    intro();
                    gameState = GAME_STATE.START_GAME;
                    break;

                // Start the game, set the number of players, names and round
                case START_GAME:

                    this.hurkleXPos = randomNumber();
                    this.hurkleYPos = randomNumber();
                    System.out.println("HURKLE AT : " + this.hurkleXPos + "," + this.hurkleYPos);

                    this.guesses = 1;
                    gameState = GAME_STATE.GUESSING;

                    break;

                // Guess an x,y position of the hurkle
                case GUESSING:
                    String guess = displayTextAndGetInput("GUESS #" + this.guesses + "? ");
                    this.playerGuessXPos = getDelimitedValue(guess, 0);
                    this.playerGuessYPos = getDelimitedValue(guess, 1);
                    if (foundHurkle()) {
                        this.gameState = GAME_STATE.PLAY_AGAIN;
                    } else {
                        showDirectionOfHurkle();
                        this.guesses++;
                        if(this.guesses > MAX_GUESSES) {
                            System.out.println("SORRY, THAT'S "
                                    + MAX_GUESSES + " GUESSES.");
                            System.out.println("THE HURKLE IS AT "
                                    + this.hurkleXPos + "," + this.hurkleYPos);
                            System.out.println();
                            this.gameState = GAME_STATE.PLAY_AGAIN;
                        }
                    }

                    break;

                case PLAY_AGAIN:
                    System.out.println("LET'S PLAY AGAIN, HURKLE IS HIDING.");
                    System.out.println();
                    this.gameState = GAME_STATE.START_GAME;
                    break;
            }
            // Effectively an endless loop because the game never quits as per
            // the original basic code.
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    private void showDirectionOfHurkle() {
        System.out.print("GO ");
        if(this.playerGuessYPos == this.hurkleYPos) {
            // don't print North or South because the player has chosen the
            // same y grid pos as the hurkle
        } else if (this.playerGuessYPos < this.hurkleYPos) {
            System.out.print("NORTH");
        } else if(this.playerGuessYPos > this.hurkleYPos) {
            System.out.print("SOUTH");
        }

        if(this.playerGuessXPos == this.hurkleXPos) {
            // don't print East or West because the player has chosen the
            // same x grid pos as the hurkle
        } else if(this.playerGuessXPos < this.hurkleXPos) {
            System.out.print("EAST");
        } else if(this.playerGuessXPos > this.hurkleXPos) {
            System.out.print("WEST");
        }
        System.out.println();
        return;
    }

    private boolean foundHurkle() {
        if ((this.playerGuessXPos - this.hurkleXPos)
                - (this.playerGuessYPos - this.hurkleYPos) == 0) {
            System.out.println("YOU FOUND HIM IN " + this.guesses + " GUESSES.");
            return true;
        }

        return false;
    }

    /**
     * Display info about the game
     */
    private void intro() {
        System.out.println("HURKLE");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("A HURKLE IS HIDING ON A " + GRID_SIZE + " BY "
                + GRID_SIZE + " GRID. HOMEBASE");
        System.out.println("ON THE GRID IS POINT 0,0 IN THE SOUTHWEST CORNER,");
        System.out.println("AND ANY POINT ON THE GRID IS DESIGNATED BY A");
        System.out.println("PAIR OF WHOLE NUMBERS SEPERATED BY A COMMA. THE FIRST");
        System.out.println("NUMBER IS THE HORIZONTAL POSITION AND THE SECOND NUMBER");
        System.out.println("IS THE VERTICAL POSITION. YOU MUST TRY TO");
        System.out.println("GUESS THE HURKLE'S GRIDPOINT. YOU GET "
                + MAX_GUESSES + " TRIES.");
        System.out.println("AFTER EACH TRY, I WILL TELL YOU THE APPROXIMATE");
        System.out.println("DIRECTION TO GO TO LOOK FOR THE HURKLE.");
    }

    /**
     * Generate random number
     * Used to create one part of an x,y grid position
     *
     * @return random number
     */
    private int randomNumber() {
        return (int) (Math.random()
                * (GRID_SIZE) + 1);
    }

    /*
     * Print a message on the screen, then accept input from Keyboard.
     *
     * @param text message to be displayed on screen.
     * @return what was typed by the player.
     */
    private String displayTextAndGetInput(String text) {
        System.out.print(text);
        return kbScanner.next();
    }

    /**
     * Accepts a string delimited by comma's and returns the pos'th delimited
     * value (starting at count 0).
     *
     * @param text - text with values separated by comma's
     * @param pos  - which position to return a value for
     * @return the int representation of the value
     */
    private int getDelimitedValue(String text, int pos) {
        String[] tokens = text.split(",");
        return Integer.parseInt(tokens[pos]);
    }
}
