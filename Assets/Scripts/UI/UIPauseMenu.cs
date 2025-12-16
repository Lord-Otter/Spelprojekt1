using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Spelprojekt1
{
    public class UIPauseMenu : MonoBehaviour
    {
        public static UIPauseMenu Instance { get; private set; }

        public enum PauseState
        {
            Gameplay,
            Paused,
            Settings,
            ConfirmExit
        }

        public PauseState CurrentState { get; private set; }

        private PlayerInput playerInput;

        [Header("UI Panels")]
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private GameObject settingsMenuPanel;
        [SerializeField] private GameObject confirmExitPanel;

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            playerInput = Object.FindAnyObjectByType<PlayerInput>();
        }

        private void Start()
        {
            SetState(PauseState.Gameplay);
        }

        public void TogglePause()
        {
            if (CurrentState == PauseState.Gameplay)
            {
                SetState(PauseState.Paused);
            }
            else
            {
                SetState(PauseState.Gameplay);
            }
        }

        public void SetState(PauseState newState)
        {
            CurrentState = newState;

            CloseAllMenus();

            switch (newState)
            {
                case PauseState.Gameplay:
                    Time.timeScale = 1f;
                    playerInput.SwitchCurrentActionMap("Player");
                    break;
                
                case PauseState.Paused:
                    Time.timeScale = 0f;
                    playerInput.SwitchCurrentActionMap("UI");
                    pauseMenuPanel.SetActive(true);
                    break;
                
                case PauseState.Settings:
                    settingsMenuPanel.SetActive(true);
                    break;
                
                case PauseState.ConfirmExit:
                    confirmExitPanel.SetActive(true);
                    break;
                
            }
        }

        public void OpenGameplay()
        {
            SetState(PauseState.Gameplay);
        }

        public void OpenPaused()
        {
            SetState(PauseState.Paused);
        }

        public void OpenSettings()
        {
            SetState(PauseState.Settings);
        }

        public void OpenConfirmExit()
        {
            SetState(PauseState.ConfirmExit);
        }

        public void ExitToMainMenu()
        {
            Time.timeScale = 1f;
            SetState(PauseState.Gameplay);
            SceneManager.LoadScene("MainMenu");
        }

        private void CloseAllMenus()
        {
            pauseMenuPanel.SetActive(false);
            settingsMenuPanel.SetActive(false);
            confirmExitPanel.SetActive(false);       
        }

        /*public void PauseGame()
        {
            if (IsPaused)
            {
                return;
            }

            IsPaused = true;
            Time.timeScale = 0f;

            playerInput.SwitchCurrentActionMap("UI");

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            OpenPauseMenu();
        }

        public void ResumeGame()
        {
            if (!IsPaused)
            {
                return;
            }

            IsPaused = false;
            Time.timeScale = 1f;

            playerInput.SwitchCurrentActionMap("Player");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            CloseAllMenus();
        }

        public void OpenPauseMenu()
        {
            CloseAllMenus();
            pauseMenuPanel.SetActive(true);
        }

        public void OpenSettingsMenu()
        {
            CloseAllMenus();
            settingsMenuPanel.SetActive(true);
        }

        public void OpenConfirmExitMenu()
        {
            CloseAllMenus();
            confirmExitPanel.SetActive(true);
        }

        public void CloseAllMenus()
        {
            pauseMenuPanel.SetActive(false);
            settingsMenuPanel.SetActive(false);
            confirmExitPanel.SetActive(false);
        }

        public void ExitToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }*/
    }
}

