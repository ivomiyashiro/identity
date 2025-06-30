import { Router } from "@/features/router/app.router";
import { ThemeProvider } from "@/features/theme/providers/theme.provider";
import { Toaster } from "./components/ui/sonner";
import { GoogleAuthProvider } from "./features/auth/providers/google-auth.context.provider";
import { QueryProvider } from "./components/providers/query.provider";
import { AuthProvider } from "./features/auth/providers/auth.provider";

function App() {
  return (
    <QueryProvider>
      <AuthProvider>
        <GoogleAuthProvider>
          <ThemeProvider defaultTheme="system" storageKey="ui-theme">
            <Router />
            <Toaster />
          </ThemeProvider>
        </GoogleAuthProvider>
      </AuthProvider>
    </QueryProvider>
  );
}

export default App;
