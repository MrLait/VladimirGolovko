import React from "react";
import s from './Header.module.css';
import {NavLink} from "react-router-dom";
import i18n from "i18next";
import {useTranslation} from "react-i18next";

const Header = (props) => {
    const {t} = useTranslation();
    const onChange = (props) => {
        i18n.changeLanguage(props.target.value);
    }
    return (
        <header className={s.header}>
            <span className={s.homeBlock}>
                <NavLink to={'/home'}>{t('Home')}</NavLink>
            </span>
            <span className={s.select}>
                <select name={'language'} onChange={onChange}>
                    <option value='en'>English</option>
                    <option value='ru'>Русский</option>
                    <option value='be'>Белорусский</option>
                </select>
            </span>
            <span className={s.authBlock}>
                {
                    props.isAuth ?
                        <span>
                        <span className={s.span}>
                            <NavLink to={'/basket'}>
                                {t('basket')}
                            </NavLink>
                        </span>
                            <span className={s.span}>{props.email}</span>
                            <span className={s.span}>
                                <button onClick={props.logout}>
                                    {t('Logout')}
                                </button>
                            </span>
                        </span>
                        : <span>
                        <span className={s.span}>
                            <NavLink to={'/register'}>
                                {t('Register')}
                            </NavLink>
                        </span>
                        <span className={s.span}>
                            <NavLink to={'/login'}>
                                {t('Login')}
                            </NavLink>
                        </span>
                        </span>
                }
            </span>
        </header>
    );
}

export default Header;