using System.Collections;
using UnityEngine;
using TMPro;


namespace Pong.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Transform ball;
        public float startSpeed = 3f;
        public GoalTrigger leftGoalTrigger;
        public GoalTrigger rightGoalTrigger;
        
        public TMP_Text leftScoreText;
        public TMP_Text rightScoreText;
        private Color[] rainbowColors = {
            Color.red,
            new Color(1f, 0.5f, 0f), 
            Color.yellow,
            Color.green,// Blue
            new Color(0.5f, 0f, 0.5f), 
            new Color(0.8f, 0.6f, 0.7f) 
        };

        int leftPlayerScore;
        int rightPlayerScore;
        Vector3 ballStartPos;

        const int scoreToWin = 7;

        //---------------------------------------------------------------------------
        void Start()
        {
            ballStartPos = ball.position;
            Rigidbody ballBody = ball.GetComponent<Rigidbody>();
            ballBody.velocity = new Vector3(1f, 0f, 0f) * startSpeed;
            UpdateScoreLeft();
            UpdateScoreRight();
        }

        //---------------------------------------------------------------------------
        public void OnGoalTrigger(GoalTrigger trigger)
        {
            // If the ball entered a goal area, increment the score, check for win, and reset the ball

            if (trigger == leftGoalTrigger)
            {
                rightPlayerScore++;
                UpdateScoreRight();
                Debug.Log($"Right player scored: {rightPlayerScore}");

                if (rightPlayerScore == scoreToWin)
                    Debug.Log("Right player wins!");
                else
                    ResetBall(-1f);
            }
            else if (trigger == rightGoalTrigger)
            {
                leftPlayerScore++;
                UpdateScoreLeft();
                Debug.Log($"Left player scored: {leftPlayerScore}");

                if (rightPlayerScore == scoreToWin)
                    Debug.Log("Right player wins!");
                else
                    ResetBall(1f);
            }
        }

        //---------------------------------------------------------------------------
        void ResetBall(float directionSign)
        {
            ball.position = ballStartPos;

            // Start the ball within 20 degrees off-center toward direction indicated by directionSign
            directionSign = Mathf.Sign(directionSign);
            Vector3 newDirection = new Vector3(directionSign, 0f, 0f) * startSpeed;
            newDirection = Quaternion.Euler(0f, Random.Range(-20f, 20f), 0f) * newDirection;

            var rbody = ball.GetComponent<Rigidbody>();
            rbody.velocity = newDirection;
            rbody.angularVelocity = new Vector3();

            // We are warping the ball to a new location, start the trail over
            ball.GetComponent<TrailRenderer>().Clear();
        }
        
        public void UpdateScoreRight()
        {
            StartCoroutine(ChangeScoreColor(rightScoreText, rightPlayerScore));
            rightScoreText.text = rightPlayerScore.ToString();
        }

        public void UpdateScoreLeft()
        {
            StartCoroutine(ChangeScoreColor(leftScoreText, leftPlayerScore));
            leftScoreText.text = leftPlayerScore.ToString();
        }
        
        private IEnumerator ChangeScoreColor(TMP_Text scoreText, int newScore)
        {
            Color originalColor = scoreText.color;
            Color targetColor = rainbowColors[newScore % rainbowColors.Length];

            float timer = 0f;
            while (timer < 0.5f)
            {
                timer += Time.deltaTime;
                float t = timer / 0.5f;
                scoreText.color = Color.Lerp(originalColor, targetColor, t);
                yield return null;
            }

            scoreText.color = targetColor;
        }
    }
}


