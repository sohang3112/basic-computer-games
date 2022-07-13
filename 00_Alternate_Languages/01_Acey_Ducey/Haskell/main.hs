import Data.Char (isSpace, toLower)
import Control.Monad (when)

playGame :: IO ()
playGame = do
  undefined

main = do
  putStrLn "Acey-Ducey is played in the following manner"
  putStrLn "The dealer (computer) deals two cards face up"
  putStrLn "You have an option to bet or not bet depending"
  putStrLn "on whether or not you feel the card will have"
  putStrLn "a value between the first two."
  putStrLn "If you do not want to bet, input a 0"
  gameLoop
  putStrLn "Ok hope you had fun"

  where 
    gameLoop = do
      playGame
      putStrLn "Try again? (yes or no) "
      action <- dropWhile isSpace <$> getLine
      when (not (null action) && toLower (head action) == 'y') gameLoop      
