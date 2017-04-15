package com.softuni.model;


public class Calculator {
    private double leftOperand;
    private double rightOperand;
    private String operator;

    public Calculator(double leftOperand, double rightOperand, String operator){
        this.leftOperand = leftOperand;
        this.rightOperand = rightOperand;
        this.operator = operator;
    }

    public double calculateResult(){
        double result;

        switch (this.operator){
            case "+":
                result = this.leftOperand + this.rightOperand;
                break;
            case "-":
                result = this.leftOperand - this.rightOperand;
                break;
            case "*":
                result = this.leftOperand * this.rightOperand;
                break;
            case "/":
                result = this.leftOperand / this.rightOperand;
                break;
            default:
                result = 0;
        }

        return result;

    }
}
