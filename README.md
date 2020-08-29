# Persistence of Vision Spherical Display with Bluetooth Control
A spherical display with Bluetooth control through a custom-made app.

**Demonstration Video:** https://youtu.be/LuZrvqpimK4 

**How it works:**
This display uses the persistence of vision phenomenon where a moving point light source presists in our vision and is precieved to be a continuos light source.
In this project, a strip of 46 LEDS are attached onto a 3D printed circle which is spun around to form a sphere. The strip of LEDs is controlled by an Arduino Nano which continuously writes the correct color onto each LED to form an image.

**Potential Use Case:**
This device showcases images which can be used for advertisement and entertainment purposes. Since a technology in this form is not yet well-known and adapted in the real world, it is quite an eye catcher. This means companies can easily gain people's curiosity by advertising their products on this display.

**Features:**
 - Bluetooth Control through app
 - Display any color
 - Display images (41x23 resolution)
 - Display text
 - Display Animations
 - Control color by orienting phone

**Components List:**
 - Arduino Nano
 - HM-10 Bluetooth Module
 - WS2812B Individually Addressable RGB LED Strip (144 LEDs/m)
 - A3144 Hall Effect Sensor
 - Magnet
 - 4 x Rechargable AA Batteries (1.25V cells)
 - DC Motor
 - 470 uF Capacitor
 - Resistors


**Info on Attached Files:**
- 3D Model: A 3D model of the globe structure made in SketchUp.
- Images: Images of the hardware, globe, android app, and windows application.
- POV_Globe_Program: The Arduino code which drives the Globe Display.
- Globe_BT_Controller: The android app apk which controls the Globe Display wirelessly through Bluetooth (Optimized for Samsung Galaxy Note 8).
