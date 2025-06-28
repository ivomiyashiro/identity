import { MoonIcon, SunIcon } from "lucide-react";
import { cn } from "@/lib/utils/cn.util";
import { useTheme } from "@/features/theme/hooks/use-theme";

export const ThemeButton = ({ className }: { className?: string }) => {
    const { theme, setTheme } = useTheme();

    return (
        <button
            onClick={() => setTheme(theme === "dark" ? "light" : "dark")}
            className={cn(
                "fixed bottom-4 right-4 flex items-center justify-center rounded-md p-2 hover:bg-muted",
                className
            )}
        >
            {theme === "dark" ? <SunIcon /> : <MoonIcon />}
        </button>
    );
};
