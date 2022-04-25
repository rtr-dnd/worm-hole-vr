#ifndef SWITCH_HPP
#define SWITCH_HPP
#include<Arduino.h>

class Switch {
    public:
        Switch(){}
        Switch(int switchPin, int LEDPin) {
            switch_pin = switchPin;
            led_pin = LEDPin;
        }
        void setup() {
            pinMode(switch_pin, INPUT_PULLUP);
            pinMode(led_pin, OUTPUT);
        }
        bool getState() {
            return digitalRead(switch_pin);
        }
        void update() {
            bool isDown = digitalRead(switch_pin);
            // Serial.println(isDown);
            if (!isDown) {
                if (!isPushed && !hasPushed) {
                    isPushed = true;
                    hasPushed = true;
                } else {
                    isPushed = false;
                }
            } else {
                isPushed = false;
                hasPushed = false;
            }
        }
        bool getIsPushed() {
            return isPushed;
        }

        void controlLED(bool isHIGH) {
            digitalWrite(led_pin, isHIGH);
        }

    private:
        int switch_pin, led_pin;
        bool isPushed = false, hasPushed = false;
};

#endif
