import React, { FC, ReactNode } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';

interface LayoutProps {
    children: ReactNode;
}

export const Layout: FC<LayoutProps> = ({ children }) => {
    return (
        <div>
            <NavMenu />
            <Container>
                {children}
            </Container>
        </div>
    );
};