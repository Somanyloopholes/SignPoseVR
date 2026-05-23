# SignPose VR

A virtual reality application for learning American Sign Language (ASL) on Meta Quest headsets, built with Unity and the Meta XR SDK. Users practice signing the ASL alphabet and digits using hand tracking with real-time visual feedback. No controllers are needed.

Published on the [Meta Horizon Store](https://www.meta.com/experiences/24069781642651333/).

![Demo](https://raw.githubusercontent.com/Somanyloopholes/SignPoseVR/main/demoGIF.gif)

**Demo Video:** [https://youtu.be/qkcpHR63A9Y](https://youtu.be/qkcpHR63A9Y)

## Features

- **Learn Mode**: View a visual prompt showing the target ASL sign along with a text description of the hand position. Get real-time feedback as you form the gesture.
- **Quiz Mode**: The reference image is hidden. Test your recall by signing from memory. A live score counter tracks correct answers.
- **36-sign vocabulary**: Covers the 26 ASL alphabet letters and digits 0 through 9, each with a hand-shape definition and reference image.
- **Randomized flashcard progression**: Poses are presented in random order with no immediate repeats for varied practice sessions.
- **Visual glow feedback**: A border around the display board glows when the headset detects the correct hand shape and confirms a match after you hold the pose steadily for 0.8 seconds.
- **Controller-free**: Built entirely around Meta Quest hand tracking.
- **Device support**: Meta Quest 2, Quest Pro, Quest 3, and Quest 3S.

## Tech Stack

- **Unity** 2022.3.60f1 (LTS)
- **C#**
- **Meta XR SDK**
- **Unity XR Hands**
- **Unity XR Interaction Toolkit**
- **OpenXR**
- **Universal Render Pipeline (URP)**
- **TextMeshPro**

## Usage

The app requires a **Meta Quest headset** (Quest 2, Pro, 3, or 3S) and is available on the [Meta Horizon Store](https://www.meta.com/experiences/24069781642651333/).

1. Put on your Quest headset and launch SignPose VR.
2. The app starts in **Learn Mode**. You see a reference image of an ASL sign and a text description of the hand position.
3. Form the displayed sign with your hand. When the headset detects a match, the border around the display board glows. Hold the pose for about 0.8 seconds to confirm.
4. The app automatically advances to the next random sign.
5. Press **Skip** to move to the next sign without matching.
6. Press **Change Mode** to switch to **Quiz Mode**. The reference image is hidden, and you sign from memory. Your score is displayed and increments with each correct match.
