import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function RegisterAdmin() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [registerStatus, setRegisterStatus] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [address, setAddress] = useState("");
    const [phoneNumber, setPhoneNumber] = useState("");
    const [dateOfBirth, setDateOfBirth] = useState("");

    const history = useNavigate();

    const handleRegister = async (event) => {
        event.preventDefault();
        if (password !== confirmPassword) {
            setRegisterStatus("Passwords do not match.");
            return;
        }
        const formData = new FormData();
        formData.append('Username', username);
        formData.append('Password', password);
        formData.append('Email', email);
        formData.append('FirstName', firstName);
        formData.append('LastName', lastName);
        formData.append('Address', address);
        formData.append('PhoneNumber', phoneNumber);
        formData.append('DateOfBirth', dateOfBirth);

        try {
            const response = await axios.post('http://localhost:5209/api/Account/RegisterAdmin', formData);
            let message = response.data.Message;
            if (!message) {
                message = JSON.parse(response.data).Message;
            }
            if (message === "Registered successfully!") {
                history.push('/login');
            } else {
                setRegisterStatus(message);
            }
        } catch (error) {
            setRegisterStatus(error.response.data.Message);
        }
    }

    return (
        <div>
            <h1>Register Admin</h1>
            <form onSubmit={handleRegister}>
                <label>
                    Username:
                    <input type="text" value={username} onChange={e => setUsername(e.target.value)} />
                </label>
                <label>
                    Password:
                    <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
                </label>
                <label>
                    Confirm Password:
                    <input type="password" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} />
                </label>
                <label>
                    Email:
                    <input type="email" value={email} onChange={e => setEmail(e.target.value)} />
                </label>
                <label>
                    First Name:
                    <input type="text" value={firstName} onChange={e => setFirstName(e.target.value)} />
                </label>
                <label>
                    Last Name:
                    <input type="text" value={lastName} onChange={e => setLastName(e.target.value)} />
                </label>
                <label>
                    Address:
                <input type="text" value={address} onChange={e => setAddress(e.target.value)} />
                </label>
                <label>
                    Phone Number:
                    <input type="text" value={phoneNumber} onChange={e => setPhoneNumber(e.target.value)} />
                </label>
                <label>
                    Date of Birth:
                    <input type="text" value={dateOfBirth} onChange={e => setDateOfBirth(e.target.value)} />
                </label>
                <input type="submit" value="Submit" />
            </form>
            <p>{registerStatus}</p>
        </div>
    );
}

export default RegisterAdmin;