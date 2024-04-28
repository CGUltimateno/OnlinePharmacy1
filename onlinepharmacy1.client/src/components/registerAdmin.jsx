import { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import styles from './Register.module.css'; // Import CSS module

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

    const navigate = useNavigate();

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
            console.log('Response:', response); // Log the response received from the server
            if (response && response.data) {
                let message = response.data.Message;
                setRegisterStatus(message);
            } else {
                throw new Error('No response from server');
            }
        } catch (error) {
            console.error('Error:', error); // Log any errors that occur
            setRegisterStatus(error.message);
        } finally {
            console.log('Redirecting...'); // Log that redirection is happening
            navigate('/'); // Redirect to login page
        }
    }
        
    return (
        <div className={styles.container}>
            <form onSubmit={handleRegister} className={styles.form}>
                <h2 className={styles.title}>Register Admin</h2>
                <input type="text" value={username} onChange={e => setUsername(e.target.value)} placeholder="Username" className={styles.input} />
                <input type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="Password" className={styles.input} />
                <input type="password" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} placeholder="Confirm Password" className={styles.input} />
                <input type="email" value={email} onChange={e => setEmail(e.target.value)} placeholder="Email" className={styles.input} />
                <input type="text" value={firstName} onChange={e => setFirstName(e.target.value)} placeholder="First Name" className={styles.input} />
                <input type="text" value={lastName} onChange={e => setLastName(e.target.value)} placeholder="Last Name" className={styles.input} />
                <input type="text" value={address} onChange={e => setAddress(e.target.value)} placeholder="Address" className={styles.input} />
                <input type="text" value={phoneNumber} onChange={e => setPhoneNumber(e.target.value)} placeholder="Phone Number" className={styles.input} />
                <input type="text" value={dateOfBirth} onChange={e => setDateOfBirth(e.target.value)} placeholder="Date of Birth" className={styles.input} />
                <button type="submit" className={styles.button}>Submit</button>
            </form>
            <p>{registerStatus}</p>
        </div>
    );
}

export default RegisterAdmin;