import { Router } from "@/features/router/app.router";
import { ThemeProvider } from "@/features/theme/providers/theme.provider";
import { Toaster } from "./components/ui/sonner";

function App() {
  return (
    <ThemeProvider defaultTheme="system" storageKey="ui-theme">
      <Router />
      <Toaster />
    </ThemeProvider>
  );
}

export default App;
