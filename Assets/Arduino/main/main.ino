#include <Arduino.h>
#include <Keyboard.h>
#include "Switch.hpp"
const int SWITCH_1 = 2;
const int SWITCH_2 = 3;
const int SWITCH_3 = 4;
const int SWITCH_4 = 5;
const int SWITCH_5 = 6;
const int LED_1 = 7;
const int LED_2 = 8;
const int LED_3 = 9;
const int LED_4 = 10;
const int LED_5 = 16;

const int RESET_PIN = 14;
uint8_t ascii_table[] = {0x30,0x31,0x32,0x33,0x34};

Switch switchs[5];
int handleSwitchs();
bool prevState[] = {true, true, true, true, true};
void handleSerialEvent();

void setup() {
  Serial.begin(9600);
  // put your setup code here, to run once:
  switchs[0] = Switch(SWITCH_1, LED_1);
  switchs[1] = Switch(SWITCH_2, LED_2);
  switchs[2] = Switch(SWITCH_3, LED_3);
  switchs[3] = Switch(SWITCH_4, LED_4);
  switchs[4] = Switch(SWITCH_5, LED_5);
  switchs[0].setup();
  switchs[1].setup();
  switchs[2].setup();
  switchs[3].setup();
  switchs[4].setup();
  pinMode(RESET_PIN,INPUT_PULLUP);
  // pinMode(LED_1,OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  switchs[0].update();
  switchs[1].update();
  switchs[2].update();
  switchs[3].update();
  switchs[4].update();
  int switch_id = handleSwitchs();
  if (switch_id > -1){
    // Serial.print("switch : ");
    // Serial.println(switch_id);
  }

  if (!digitalRead(RESET_PIN)){
      for(int i = 0;i<5;i++) {
          switchs[i].controlLED(false);
      }
      Keyboard.releaseAll();
  }

  if (Serial.available()) {
    handleSerialEvent();
  }
  delay(1);
}

int handleSwitchs() {
    for (int i = 0;i<5;i++) {
      bool currentState = switchs[i].getState();
      if (!currentState) {
        if (prevState[i]) {
          Keyboard.press(ascii_table[i]);
          // Serial.print("press");
          // Serial.println(i);
          return i;
        }
      } else {
        // Serial.print("release");
        // Serial.println(i);
        Keyboard.release(ascii_table[i]);
      }
      prevState[i] = currentState;
    }
  return -1;
}

void handleSerialEvent() {
  char cmd = Serial.read();
  switch(cmd) {
    case 't':
      for(int i = 0;i<5;i++) {
          switchs[i].controlLED(false);
      }
      break;
    case 'g':
      digitalWrite(LED_1,HIGH);
      digitalWrite(LED_2,HIGH);
      digitalWrite(LED_3,HIGH);
      digitalWrite(LED_4,HIGH);
      digitalWrite(LED_5,HIGH);
      break;
    default:
      break;
  }
}
