import{ useState } from 'react';
import axios from 'axios';
import styles from './Register.module.css'; // Import CSS module

function Register() {
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        dateOfBirth: '',
        phoneNumber: '',
        address: '',
        username: '',
        email: '',
        password: '',
        role: 'user'
    });

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const form = new FormData();
            for (const key in formData) {
                if (key === 'dateOfBirth') {
                    // Convert the dateOfBirth string to a DateTime string
                    const dateOfBirth = new Date(formData[key]);
                    form.append(key, dateOfBirth.toISOString().split('T')[0]);
                } else {
                    form.append(key, formData[key]);
                }
            }
            const response = await axios.post('http://localhost:5209/api/Account', form);
            if (response.status === 200) {
                window.location.href = '/login';
            }
        } catch (error) {
            if (error.response && error.response.status === 409) {
                alert(error.response.data.message);
            } else {
                console.error(error);
            }
        }
    };

    return (
        <div className={styles.container}>
            <form onSubmit={handleSubmit} className={styles.form}>
                <h2 className={styles.title}>Register</h2>
                <input name="firstName" onChange={handleChange} placeholder="First Name" className={styles.input} />
                <input name="lastName" onChange={handleChange} placeholder="Last Name" className={styles.input} />
                <input name="dateOfBirth" onChange={handleChange} placeholder="Date of Birth" className={styles.input} />
                <input name="phoneNumber" onChange={handleChange} placeholder="Phone Number" className={styles.input} />
                <input name="address" onChange={handleChange} placeholder="Address" className={styles.input} />
                <input name="username" onChange={handleChange} placeholder="Username" className={styles.input} />
                <input name="email" onChange={handleChange} placeholder="Email" className={styles.input} />
                <input name="password" onChange={handleChange} placeholder="Password" type="password" className={styles.input} />
                <select name="role" onChange={handleChange} className={styles.select}>
                    <option value="user">User</option>
                    <option value="admin">Admin</option>
                </select>
                <button type="submit" className={styles.button}>Register</button>
            </form>
        </div>
    );
}

export default Register;
