int analogPin = A0;
int directionPin = 4;
int activationPin = 3;
bool direction = true;
bool activation = true;

int read_analog(){
  int sensorValue = analogRead(analogPin);
  return sensorValue;
}

void flip_direction(){
  if(direction){
    digitalWrite(directionPin, LOW);
    direction = false;
  }
  else{
    digitalWrite(directionPin, HIGH);
    direction = true;
  }
}

void switch_activation(){
  if(activation){
    digitalWrite(activationPin, LOW);
    activation = false;
  }
  else{
    digitalWrite(activationPin, HIGH);
    activation = true;
  }
}

void cap(){
  //Capping function
  Serial.println("Capping function is running");
  
  flip_direction();
  
  while(true){
    int out = read_analog();
    if(out>170){
      Serial.println(out);
      switch_activation();
      delay(100);
      flip_direction();
      delay(100);
      switch_activation();
      break;
    }
  }
}

void decap(){
  //Decapping function
  Serial.println("Deapping function is running");
  flip_direction();
  delay(2000);
  flip_direction();
}

void setup() {
  Serial.begin(9600);
  pinMode(directionPin, OUTPUT);
  pinMode(activationPin, OUTPUT);

  digitalWrite(activationPin, LOW);
  digitalWrite(directionPin, HIGH);
  while(!Serial);

  switch_activation();
}

void loop() {
  if(Serial.available() > 0){
    char input = Serial.read();

    switch(input){
      case '1':
        cap();
        break;
      case '2':
        decap();
        break;
    }
  }
  delay(500);
}
  
