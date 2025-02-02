﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const container = document.getElementById('container');
const registerBtn = document.getElementById('register');
const loginBtn = document.getElementById('login');
const panel = document.getElementById("panel");
const sanyika = document.getElementById("sanyika");
const signin = document.getElementById("signin");
const signup = document.getElementById("signup");
const left = document.getElementById("left");
const right = document.getElementById("right");
const toggle = document.getElementById("toggle");

registerBtn.addEventListener('click', () => {
    panel.classList.add("active");
    signin.classList.add("active");
    signup.classList.add("active");
    right.classList.add("active");
    left.classList.add("active");
    toggle.classList.add("active");
});

loginBtn.addEventListener('click', () => {
    panel.classList.remove("active");
    signin.classList.remove("active");
    signup.classList.remove("active");
    right.classList.remove("active");
    left.classList.remove("active");
    toggle.classList.remove("active");
});

