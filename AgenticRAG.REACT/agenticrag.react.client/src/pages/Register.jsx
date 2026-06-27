import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import styles from './Auth.module.css';

export default function Register() {
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password: '',
        agreeTerms: false
    });

    const handleChange = (e) => {
        const { id, value, type, checked } = e.target;
        setFormData(prev => ({
            ...prev,
            [id]: type === 'checkbox' ? checked : value
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log('Registration submitted:', formData);
    };

    return (
        <div className={styles.authWrapper}>
            <div className={styles.authCard}>
                <div className={styles.logoSection}>
                    <span className={styles.logoIcon}>🍇</span>
                    <span className={styles.logoText}>BERRY</span>
                </div>

                <h2 className={styles.authTitle}>Sign up</h2>
                <p className={styles.authSubtitle}>Create your account to get started</p>

                <form onSubmit={handleSubmit} className={styles.authForm}>
                    <div className={styles.row}>
                        <div className={styles.inputGroup}>
                            <label htmlFor="firstName" className={styles.label}>First Name</label>
                            <input
                                type="text"
                                id="firstName"
                                className={styles.input}
                                placeholder="John"
                                value={formData.firstName}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div className={styles.inputGroup}>
                            <label htmlFor="lastName" className={styles.label}>Last Name</label>
                            <input
                                type="text"
                                id="lastName"
                                className={styles.input}
                                placeholder="Doe"
                                value={formData.lastName}
                                onChange={handleChange}
                                required
                            />
                        </div>
                    </div>

                    <div className={styles.inputGroup}>
                        <label htmlFor="email" className={styles.label}>Email Address</label>
                        <input
                            type="email"
                            id="email"
                            className={styles.input}
                            placeholder="john.doe@example.com"
                            value={formData.email}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div className={styles.inputGroup}>
                        <label htmlFor="password" className={styles.label}>Password</label>
                        <input
                            type="password"
                            id="password"
                            className={styles.input}
                            placeholder="••••••••"
                            value={formData.password}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div className={styles.formOptions}>
                        <label className={styles.checkboxLabel}>
                            <input
                                type="checkbox"
                                id="agreeTerms"
                                checked={formData.agreeTerms}
                                onChange={handleChange}
                                required
                            />
                            <span>I agree to the <a href="#terms" className={styles.forgotLink}>Terms & Conditions</a></span>
                        </label>
                    </div>

                    <button type="submit" className={styles.submitBtn}>Sign Up</button>
                </form>

                <div className={styles.authFooter}>
                    Already have an account? <Link to="/login" className={styles.switchLink}>Sign In</Link>
                </div>
            </div>
        </div>
    );
}