import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import styles from './Auth.module.css';

export default function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [rememberMe, setRememberMe] = useState(false);

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log('Login submitted:', { email, password, rememberMe });
    };

    return (
        <div className={styles.authWrapper}>
            <div className={styles.authCard}>
                <div className={styles.logoSection}>
                    <span className={styles.logoIcon}>🍇</span>
                    <span className={styles.logoText}>BERRY</span>
                </div>

                <h2 className={styles.authTitle}>Hi, Welcome Back</h2>
                <p className={styles.authSubtitle}>Enter your credentials to continue</p>

                <form onSubmit={handleSubmit} className={styles.authForm}>
                    <div className={styles.inputGroup}>
                        <label htmlFor="email" className={styles.label}>Email Address</label>
                        <input
                            type="email"
                            id="email"
                            className={styles.input}
                            placeholder="name@example.com"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
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
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>

                    <div className={styles.formOptions}>
                        <label className={styles.checkboxLabel}>
                            <input
                                type="checkbox"
                                checked={rememberMe}
                                onChange={(e) => setRememberMe(e.target.checked)}
                            />
                            <span>Keep me logged in</span>
                        </label>
                        <a href="#forgot" className={styles.forgotLink}>Forgot Password?</a>
                    </div>

                    <button type="submit" className={styles.submitBtn}>Sign In</button>
                </form>

                <div className={styles.authFooter}>
                    Don't have an account? <Link to="/register" className={styles.switchLink}>Sign Up</Link>
                </div>
            </div>
        </div>
    );
}